using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ReflectionVsDynamicPerformance;

[SimpleJob(launchCount: 1, warmupCount: 3, targetCount: 10, runtimeMoniker: RuntimeMoniker.Net60)]
[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
public class Benchmarks
{
	private const int GuidCount = 1_000_000;
	private readonly List<IDto> _dtos;
	private readonly List<Dto<List<string>>> _dtoList;


	public Benchmarks()
	{
		var random = new Random(DateTimeOffset.UtcNow.Millisecond);

		var guids = Enumerable.Range(0, GuidCount).Select(_ => Guid.NewGuid().ToString("N")[..random.Next(1, 32)]);

		var chunks = guids.Chunk(GuidCount / 512);

		_dtos = new();
		_dtoList = new();

		foreach (var chunk in chunks)
		{
			_dtoList.Add(new(chunk.ToList()));

			switch (random.Next(0, 4))
			{
				case 0:
					_dtos.Add(new Dto<string[]>(chunk.ToArray()));
					break;
				case 1:
					_dtos.Add(new Dto<List<string>>(chunk.ToList()));
					break;
				case 2:
					_dtos.Add(new Dto<IList<string>>(chunk.ToList()));
					break;
				case 3:
					_dtos.Add(new Dto<IEnumerable<string>>(chunk.ToList()));
					break;
			}
		}



	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public long Baseline()
	{
		long totalLength = 0;
		foreach (var dto in _dtoList)
		{
			totalLength += dto.Payload.Sum(x => x.Length);
		}

		return totalLength;
	}



	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public long SwitchCaseCast()
	{
		long totalLength = 0;
		foreach (var dto in _dtos)
		{
			switch (dto)
			{
				case Dto<List<string>> list:
					totalLength += list.Payload.Sum(x => x.Length);
					continue;
				case Dto<IList<string>> list:
					totalLength += list.Payload.Sum(x => x.Length);
					continue;
				case Dto<string[]> list:
					totalLength += list.Payload.Sum(x => x.Length);
					continue;
				case Dto<IEnumerable<string>> list:
					totalLength += list.Payload.Sum(x => x.Length);
					continue;
			}
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public long UsingReflection()
	{
		long totalLength = 0;

		foreach (var dto in _dtos)
		{
			var payload = dto.GetType().GetProperty(nameof(Dto<string>.Payload));
			totalLength += ((IEnumerable<string>)payload.GetValue(dto)).Sum(x => x.Length);
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public long UsingDynamic()
	{
		long totalLength = 0;
		foreach (var dto in _dtos)
		{
			var payload = (IEnumerable<string>)((dynamic)dto).Payload;
			totalLength += payload.Sum(x => x.Length);
		}

		return totalLength;
	}
}