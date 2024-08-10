Creates constructor for IComponent classes in [friflo ECS framework](https://github.com/friflo/Friflo.Engine.ECS). 

```csharp
public partial struct Velocity : IComponent
{
    public float Value;
}
```

Will result in this auto generated constructor:

```csharp
public Velocity(float value)
{
    this.Value = value;
}
```

Code is based on https://github.com/Flavien/quickconstructor
