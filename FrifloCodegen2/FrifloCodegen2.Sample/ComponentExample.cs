using Friflo.Engine.ECS;

namespace FrifloCodegen.Tests;

public partial struct TestComponent : IComponent
{
  public int Value;
}

public partial struct TestComponentNoArguments : IComponent
{
}

public partial struct TestComponentWithType : IComponent
{
  public Entity Entity;
}

public partial struct TestComponentMultiArguments : IComponent
{
  public int Value;
  public int Value2;
  public float Value3;
  public int Value4;
}

public partial struct TestComponentWithTestClass : IComponent
{
  public TestClass TestClass;
}

public class TestClass
{
  public int A;
}

public class Tests
{
  public void Setup()
  {
    var world = new EntityStore();
    var c = new TestComponentMultiArguments(1, 2, 3.0f, 4);
    var withType = new TestComponentWithType(world.CreateEntity());
    var withTestClass = new TestComponentWithTestClass(new());

    var tclass = new TestClass();
    
    c.Value++;
  }
}