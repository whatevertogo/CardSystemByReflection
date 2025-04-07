# CardSystem - 卡牌系统框架

[English Version](#english-version) | [中文版本](#中文版本)

## 中文版本

### 项目简介
CardSystem 是一个基于 Unity 的可扩展卡牌游戏框架，提供了灵活的卡牌效果系统和完整的数据管理功能。该框架采用组件化设计，支持快速开发各类卡牌游戏。

### 特性
- 数据驱动的卡牌系统
- 灵活的效果注册和执行机制
- 支持自定义卡牌类型和效果
- 完整的卡牌生命周期管理
- 基于ScriptableObject的数据存储
- 自动化的效果注册系统

### 系统架构
```
CardSystem/
├── Core/           # 核心系统
│   ├── Attributes/ # 特性定义
│   ├── Data/      # 数据结构
│   ├── Factory/   # 工厂类
│   ├── Interfaces/# 接口定义
│   └── Managers/  # 管理器
├── Effects/        # 效果实现
├── Runtime/        # 运行时组件
└── Usage/          # 使用示例
```

### 快速开始

1. **创建卡牌数据**
   - 在Project窗口中右键
   - 选择 Create > Card System > Card Data
   - 填写卡牌基本信息和效果

2. **创建效果**
   ```csharp
   [CardEffect("MyEffect")]
   public class MyEffect : IEffectLogic
   {
       public void Execute(ICardTarget target, EffectData data)
       {
           // 实现效果逻辑
       }
   }
   ```

3. **在场景中使用**
   ```csharp
   // 初始化系统
   CardSystemManager.Instance.Initialize();
   
   // 创建卡牌实例
   var card = CardSystemManager.Instance.CreateCardInstance("cardId");
   ```

### 注意事项
- 使用前确保正确初始化 CardSystemManager
- 自定义效果需要使用 CardEffect 特性标记
- 所有效果逻辑类都需要实现 IEffectLogic 接口

---

## English Version

### Introduction
CardSystem is a flexible card game framework for Unity that provides an extensible effect system and comprehensive data management. The framework uses a component-based design to support rapid development of various card games.

### Features
- Data-driven card system
- Flexible effect registration and execution
- Support for custom card types and effects
- Complete card lifecycle management
- ScriptableObject-based data storage
- Automated effect registration system

### Architecture
```
CardSystem/
├── Core/           # Core system
│   ├── Attributes/ # Attribute definitions
│   ├── Data/      # Data structures
│   ├── Factory/   # Factories
│   ├── Interfaces/# Interfaces
│   └── Managers/  # Managers
├── Effects/        # Effect implementations
├── Runtime/        # Runtime components
└── Usage/          # Usage examples
```

### Quick Start

1. **Create Card Data**
   - Right-click in Project window
   - Select Create > Card System > Card Data
   - Fill in card information and effects

2. **Create Effect**
   ```csharp
   [CardEffect("MyEffect")]
   public class MyEffect : IEffectLogic
   {
       public void Execute(ICardTarget target, EffectData data)
       {
           // Implement effect logic
       }
   }
   ```

3. **Use in Scene**
   ```csharp
   // Initialize system
   CardSystemManager.Instance.Initialize();
   
   // Create card instance
   var card = CardSystemManager.Instance.CreateCardInstance("cardId");
   ```

### Important Notes
- Ensure CardSystemManager is properly initialized before use
- Custom effects must be marked with CardEffect attribute
- All effect logic classes must implement IEffectLogic interface