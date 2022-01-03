using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ReflectionVsDynamicPerformance;

[SimpleJob(launchCount: 1, warmupCount: 3, targetCount: 10, runtimeMoniker: RuntimeMoniker.Net60)]
[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
public class Benchmarks
{
	private const int GuidCount = 10_000_000;


	private static readonly Dictionary<Type, PropertyInfo> PropertyCache = new();
	private readonly List<Dto<List<string>>> _dtoList;
	private readonly List<IDto> _dtos;


	public Benchmarks()
	{
		var random = new Random(DateTimeOffset.UtcNow.Millisecond);

		var guids = Enumerable.Range(0, GuidCount).Select(_ => Guid.NewGuid().ToString("N")[..random.Next(1, 32)]);

		var chunks = guids.Chunk(GuidCount / 2048);

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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long Baseline()
	{
		long totalLength = 0;
		foreach (var dto in _dtoList)
		{
			totalLength += dto.Payload.Count;
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long SwitchCaseCast()
	{
		long totalLength = 0;
		foreach (var dto in _dtos)
		{
			switch (dto)
			{
				case Dto<List<string>> list:
					totalLength += list.Payload.Count;
					continue;
				case Dto<IList<string>> list:
					totalLength += list.Payload.Count;
					continue;
				case Dto<string[]> list:
					totalLength += list.Payload.Length;
					continue;
				case Dto<IEnumerable<string>> list:
					totalLength += list.Payload.Count();
					continue;
			}
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long UsingReflection()
	{
		long totalLength = 0;

		foreach (var dto in _dtos)
		{
			var payload = dto.GetType().GetProperty(nameof(Dto<string>.Payload));
			totalLength += ((IEnumerable<string>)payload.GetValue(dto)).Count();
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long ReflectionWithCaching()
	{
		long totalLength = 0;

		foreach (var dto in _dtos)
		{
			var type = dto.GetType();
			if (!PropertyCache.TryGetValue(type, out var propertyInfo))
			{
				propertyInfo = type.GetProperty(nameof(Dto<string>.Payload));
				PropertyCache.Add(type, propertyInfo);
			}

			totalLength += ((IEnumerable<string>)propertyInfo.GetValue(dto)).Count();
		}

		return totalLength;
	}


	[Benchmark]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long UsingDynamic()
	{
		long totalLength = 0;
		foreach (var dto in _dtos)
		{
			var payload = (IEnumerable<string>)((dynamic)dto).Payload;
			totalLength += payload.Count();
		}

		return totalLength;
	}
}