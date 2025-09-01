# MK.Entities

A lightweight Entity Component System (ECS) framework for Unity, providing clean separation of data and logic with automatic lifecycle management.

## Features

- **Simple ECS Architecture** - Entities, Components, Systems with clear separation
- **Automatic Lifecycle Management** - WorldRunner MonoBehaviour handles Unity update cycles
- **System Ordering** - Control execution order with attributes
- **Entity Command Buffer** - Safe deferred entity modifications
- **Efficient Queries** - Collector pattern for fast entity filtering
- **Unity Integration** - Seamless GameObject linking

## Quick Start

```csharp
using MK.Entities;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private World world;
    
    void Start()
    {
        // Create world with automatic WorldRunner GameObject
        var systemProvider = new SystemProvider();
        world = World.NewWorld(systemProvider, "MyGame", autoUpdate: true);
        
        // Add systems
        world.AddSystem<MovementSystem>();
        world.AddSystem<HealthSystem>();
        
        // Build and start
        world.BuildWorld();
        
        // Create entities
        var player = world.EntityManager.CreateEntity();
        world.EntityManager.AddComponent(player, new HealthComponent { Current = 100, Max = 100 });
        world.EntityManager.AddComponent(player, new PositionComponent { X = 0, Y = 0 });
    }
    
    void OnDestroy()
    {
        world?.DestroyWorld();
    }
}
```

## Core Concepts

### Components

Pure data containers implementing `IComponent`:

```csharp
public struct HealthComponent : IComponent
{
    public int Current;
    public int Max;
}

public struct PositionComponent : IComponent
{
    public float X;
    public float Y;
    public float Z;
}
```

### Systems

Logic processors implementing `ISystem`:

```csharp
public class HealthSystem : ISystem
{
    private Collector<HealthComponent> healthEntities;
    
    public void OnCreate(World world)
    {
        healthEntities = world.EntityManager.RequireAll<HealthComponent>();
    }
    
    public void OnUpdate(World world)
    {
        foreach (var health in healthEntities.Components)
        {
            if (health.Current <= 0)
            {
                // Handle death
            }
        }
    }
    
    public void OnCleanUp(World world) { }
}
```

### System Ordering

Control execution order with attributes:

```csharp
[SystemOrder(10)]  // Lower numbers run first
[UpdateOrder(UpdateOrder.PreUpdate)]  // Execution phase
public class InputSystem : ISystem
{
    public void OnUpdate(World world)
    {
        // Runs at order 10 during PreUpdate
    }
}
```

#### Update Order Phases

1. **PreUpdate** - Input and initialization
2. **Update** - Main game logic
3. **FixedUpdate** - Physics updates (50Hz by default)
4. **LateUpdate** - Camera and rendering

### Entity Queries

Use Collectors for efficient entity filtering:

```csharp
public class MovementSystem : ISystem
{
    private Collector<PositionComponent, VelocityComponent> movables;
    
    public void OnCreate(World world)
    {
        // Query entities with both Position AND Velocity
        movables = world.EntityManager.RequireAll<PositionComponent, VelocityComponent>();
    }
    
    public void OnUpdate(World world)
    {
        foreach (var (entity, pos, vel) in movables.Components)
        {
            pos.X += vel.X * Time.deltaTime;
            pos.Y += vel.Y * Time.deltaTime;
            
            // Update component
            world.EntityManager.SetComponent(entity, pos);
        }
    }
}
```

### Entity Command Buffer

Deferred operations for safe entity modifications:

```csharp
public class DamageSystem : ISystem
{
    public void OnUpdate(World world)
    {
        // Commands are queued, not executed immediately
        world.EntityManager.AddComponent(entity, new DamageComponent { Amount = 10 });
        world.EntityManager.RemoveComponent<ShieldComponent>(entity);
        
        // Commands execute at frame boundaries
    }
}
```

## Architecture

### Execution Flow

```
Frame Start
    ├── BeginFrame() - PlaybackECB
    ├── PreUpdate Systems
    ├── Update Systems
    ├── FixedUpdate Systems (on physics tick)
    ├── LateUpdate Systems
    └── EndFrame() - PlaybackECB
```

### World Lifecycle

1. **Create World** - Creates WorldRunner GameObject when autoUpdate=true
2. **Add Systems** - Register systems in any order
3. **Build World** - Sorts systems and calls OnCreate
4. **Update Loop** - WorldRunner MonoBehaviour handles Unity lifecycle
5. **Destroy World** - Cleanup systems and GameObject

## API Reference

### World

```csharp
// Creation
World.NewWorld(ISystemProvider provider, string name = "World", bool autoUpdate = true)

// System Management
void AddSystem<TSystem>()
void BuildWorld()
void DestroyWorld()

// Properties
EntityManager EntityManager { get; }
IWorldRunner WorldRunner { get; }   // Pause/Resume control
WorldRunner Runner { get; }          // Access to MonoBehaviour

// Fixed Update Configuration
void SetFixedUpdateRate(float rate)  // Default: 50Hz
```

### EntityManager

```csharp
// Entity Operations
Entity CreateEntity()

// Component Operations (deferred)
void AddComponent<T>(Entity entity, T component)
void SetComponent<T>(Entity entity, T component)
void RemoveComponent<T>(Entity entity)

// Queries
Collector<T> RequireAll<T>()
Collector<T1,T2> RequireAll<T1,T2>()
// ... up to 10 components

// GameObject Linking
void Link(Entity entity, GameObject obj)
void Unlink(GameObject obj)
```

### Attributes

```csharp
[SystemOrder(int order)]           // Execution priority
[UpdateOrder(UpdateOrder phase)]   // Update phase
```

### WorldRunner

The WorldRunner is a MonoBehaviour that drives the world update loop:

```csharp
// Pause/Resume via IWorldRunner interface
world.WorldRunner.Pause();
world.WorldRunner.Resume();

// Configure physics update rate
world.SetFixedUpdateRate(60f);  // 60Hz physics

// Access MonoBehaviour if needed
world.Runner.gameObject.name = "MyWorld";
```

## Best Practices

### Component Design

- Keep components small and focused
- Use structs for value types
- Avoid references when possible
- No logic in components

### System Design

- Single responsibility per system
- Use Collectors for queries
- Avoid storing state in systems
- Order dependencies explicitly

### Performance

- Minimize SetComponent calls
- Batch similar operations
- Use appropriate UpdateOrder
- Profile with Unity Profiler

## Advanced Patterns

### Tags

```csharp
public struct PlayerTag : IComponent { }
public struct EnemyTag : IComponent { }
```

### Events

```csharp
public struct DamageEvent : IComponent
{
    public Entity Target;
    public int Amount;
}
```

### Archetypes

```csharp
public static class Archetypes
{
    public static Entity CreatePlayer(EntityManager em)
    {
        var entity = em.CreateEntity();
        em.AddComponent(entity, new PlayerTag());
        em.AddComponent(entity, new HealthComponent { Current = 100, Max = 100 });
        em.AddComponent(entity, new PositionComponent());
        return entity;
    }
}
```

## Limitations

- Not suitable for >5000 entities (use Unity DOTS)
- Components stored in Dictionary (memory trade-off)
- No built-in serialization
- Single-threaded execution

## License

Part of MikeECS Framework