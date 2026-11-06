// XyEnvironment.cs ** 11-4-26 gordon zuehlke

using System.Collections.Generic;
namespace aima;

public static class Env
{
  public enum Type { Wall, Agent, Dirt, Gold }
  public enum Direction { North, South, East, West }

  private static int _counter = 0;
  private static int NextId() => ++_counter;


  public readonly struct TypeRef : IComparable<TypeRef>, IEquatable<TypeRef>
  {
    public readonly Type Type;
    public readonly int Id;

    public TypeRef(Type t)
    {
      Type = t;
      Id = NextId();
    }

    public override string ToString()
    {
      string name = (Type == Type.Agent) ? "xyagent" : "wall";
      return "[" + name + ", " + Id + "]";
    }

    public int CompareTo(TypeRef other) => Id.CompareTo(other.Id);
    public bool Equals(TypeRef other) => Id == other.Id;
    public override bool Equals(object? obj) => obj is TypeRef o && Equals(o);
    public override int GetHashCode() => Id;
    public static bool operator <(TypeRef a, TypeRef b) => a.Id < b.Id;
    public static bool operator >(TypeRef a, TypeRef b) => a.Id > b.Id;
    public static bool operator ==(TypeRef a, TypeRef b) => a.Id == b.Id;
    public static bool operator !=(TypeRef a, TypeRef b) => a.Id != b.Id;
  }


  public interface IGameObject 
  { 
    TypeRef Ref { get; } 
  }


  public struct Gold : IGameObject 
  { 
    public TypeRef Ref { get; } 

    public Gold()
    {
      Ref = new TypeRef(Type.Gold);
    }
  }


  public struct Dirt : IGameObject 
  { 
    public TypeRef Ref { get; }

    public Dirt()
    {
      Ref = new TypeRef(Type.Dirt);
    }
  }


  public struct Wall : IGameObject 
  { 
    public TypeRef Ref { get; } 

    public Wall()
    {
      Ref = new TypeRef(Type.Wall);
    }
  }


  public struct Agent : IGameObject 
  {
    public bool IsAlive = true;
    public TypeRef Ref { get; } 

    public Agent()
    {
      Ref = new TypeRef(Type.Agent);
    }
  }
    
  
  public readonly record struct XyLocation(int X, int Y) : IComparable<XyLocation>
  {
    public int CompareTo(XyLocation other)
    {
      int c = X.CompareTo(other.X);
      return c != 0 ? c : Y.CompareTo(other.Y);
    }
  }

  
  public struct XyEnvironment
  {
    public uint W;
    public uint H;
    public SortedSet<TypeRef> Tiles;
    public SortedDictionary<int, XyLocation> Locations;
    public SortedDictionary<XyLocation, SortedSet<TypeRef>> XySpace;
  }
  

  public static XyEnvironment CreateXyEnvironment(int w, int h)
  {
    if (w <= 0 || h <= 0) 
      throw new ArgumentOutOfRangeException
        (w <= 0 ? nameof(w) : nameof(h), "params must be > 0");

    var env = new XyEnvironment
    {
      W = (uint)w,
      H = (uint)h,
      Tiles = new SortedSet<TypeRef>(),
      Locations = new SortedDictionary<int, XyLocation>(),
      XySpace = new SortedDictionary<XyLocation, SortedSet<TypeRef>>()
    };

    for (int x = 1; x <= w; x++)
      for (int y = 1; y <= h; y++)
        env.XySpace.Add(new XyLocation(x, y), new SortedSet<TypeRef>());

    return env;
  }

  
  public static bool InBounds(XyEnvironment env, XyLocation loc)
  {
    return loc.X >= 1 && loc.Y >= 1 &&
      loc.X <= env.W &&
      loc.Y <= env.H;         
  }

  
  public static bool AddObjectToLocation(TypeRef @ref, XyEnvironment env, XyLocation loc)
  {
    if (!env.XySpace.TryGetValue(loc, out SortedSet<TypeRef> objects)) return false;

    /*if (!RegisterObject(env.Objects, @ref))
    {
    //System.Console.Out.Write("UNREGISTERED" + "\n");
    return false;
  }*/

    objects.Add(@ref);
    env.Locations[@ref.Id] = loc;
    return true;
  }

  
  public static TypeRef? AddGameObject<T>(ref XyEnvironment env, XyLocation xy) where T : struct, IGameObject
  {
    T obj = new();

    if (!AddObjectToLocation(obj.Ref, env, xy)) 
      return null;

    return obj.Ref;
  }

  
  public static TypeRef? AddAgent(ref XyEnvironment env, XyLocation xy) 
  {
    return AddGameObject<Agent>(ref env, xy);
  }


}





