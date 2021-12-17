namespace ReflectionVsDynamicPerformance;


public sealed class Dto<T>: IDto
{
	public Dto(T payload)
	{
		Payload = payload;
	}


	public T Payload { get; set; }
}

public interface IDto
{
}