// XyEnvironmentTest.cs Gordon Zuehlke 11-4-26

using static aima.Env;
namespace aima.tests;

public class XyEnvironmentTest : IDisposable
{
	private TypeRef? _agent;
	private XyEnvironment _env;
	private readonly XyLocation _source;

	public XyEnvironmentTest()
	{
		_env = CreateXyEnvironment(10, 12);
		_source = new XyLocation(2, 2);
		_agent = AddAgent(ref _env, _source);
	}

	public void Dispose() { }

	[Fact]
	public void TestConstructXyEnvironment()
	{
		Assert.Equal(10u, _env.W);
		Assert.Equal(12u, _env.H);
		Assert.Equal(120, _env.XySpace.Count);
	}

	[Theory]
	[InlineData(0, 5)]
	[InlineData(5, 0)]
	[InlineData(-1, 5)]
	public void TestXyEnvironmentRejectsNonPositveParams(int w, int h)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => CreateXyEnvironment(w, h));
	}

	[Fact]
	public void TestBoundsAreOneBasedAndInclusive()
	{
		Assert.True(InBounds(_env, new XyLocation(1, 1)));
		Assert.True(InBounds(_env, new XyLocation(4, 4)));
		Assert.False(InBounds(_env, new XyLocation(0, 1)));
		Assert.False(InBounds(_env, new XyLocation(1, 0)));
		Assert.False(InBounds(_env, new XyLocation(-1, 1)));
		Assert.False(InBounds(_env, new XyLocation(1, -1)));
		Assert.False(InBounds(_env, new XyLocation(11, 1)));
		Assert.False(InBounds(_env, new XyLocation(1, 14)));
	}

	[Fact]
	public void TestAddObject()
	{
		Assert.NotNull(_agent);
		Assert.Equal(_source, GetCurrentLocationForObject(_agent.Value, _env));
	}


}































