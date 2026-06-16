# WPF 的 DataContext 分层机制

------

**1. 可视化树结构**

```c
Window (DataContext = MainWindowVM)
  └── ContentControl
        └── HomeView (DataContext = HomeVM)  ← 手动设置的
              └── ItemsControl
                    └── ScrollViewer
                          └── StackPanel (ItemsHost)
                                ├── DataTemplate (DataContext = ChatMessage[0])
                                ├── DataTemplate (DataContext = ChatMessage[1])
                                └── DataTemplate (DataContext = ChatMessage[2])
```

------

**2. DataContext 传递规则**

**规则1：父传子**

```xaml
<StackPanel DataContext="{Binding A}">  <!-- DataContext = A -->
    <TextBlock Text="{Binding B}"/>    <!-- 自动找 A.B -->
</StackPanel>
```

子控件默认继承父控件的 DataContext。

**规则2：绑定会"向下穿透"**

```xaml
<UserControl DataContext="{Binding HomeVM}">
    <ItemsControl ItemsSource="{Binding Messages}">
        <!-- 这里还是找 HomeVM.Messages -->
    </ItemsControl>
</UserControl>
```

------

**3. DataTemplate 的特殊性**

**关键点：DataTemplate 会改变 DataContext**

```xaml
<!-- ItemsControl 的 DataContext = HomeVM -->
<ItemsControl ItemsSource="{Binding Messages}">
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <!-- DataTemplate 的 DataContext = 当前遍历的那一个 ChatMessage -->
            <TextBlock Text="{Binding Content}"/>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```

------

**4. 具体执行流程**

```c#
// 你的代码
HomeVM homeVM = new HomeVM();
homeVM.Messages.Add(new ChatMessage { Content = "你好", IsSelf = true });
homeVM.Messages.Add(new ChatMessage { Content = "在吗", IsSelf = false });
```

**对应到界面：**

```c#
ItemsControl 开始工作
    ↓
找到 DataContext（HomeVM）
    ↓
取 Messages 属性（2 项）
    ↓
遍历第 1 项
    ↓
创建 DataTemplate
    设置 DataTemplate.DataContext = Messages[0] (ChatMessage { Content="你好" })
    ↓
渲染 TextBlock
    {Binding Content} → 当前 DataContext.Content → "你好"
    ↓
遍历第 2 项
    ↓
创建 DataTemplate
    设置 DataTemplate.DataContext = Messages[1] (ChatMessage { Content="在吗" })
    ↓
渲染 TextBlock
    {Binding Content} → 当前 DataContext.Content → "在吗"
```

------

**5. 如果要访问 HomeVM 的属性怎么办？**

用 `RelativeSource`：

```xaml
<DataTemplate>
    <!-- 访问当前 ChatMessage 的属性 -->
    <TextBlock Text="{Binding Content}"/>
    
    <!-- 访问 HomeVM 的属性（往上找） -->
    <TextBlock Text="{Binding DataContext.SomeProperty, 
                      RelativeSource={RelativeSource AncestorType=ItemsControl}}"/>
</DataTemplate>
```

------

**总结：**

| 层级         | DataContext  | 谁设置的                |
| :----------- | :----------- | :---------------------- |
| Window       | MainWindowVM | 代码里设置的            |
| HomeView     | HomeVM       | 构造函数里设置的        |
| DataTemplate | ChatMessage  | ItemsControl 自动设置的 |

**DataTemplate 是特殊的，它会自动把 DataContext 换成当前遍历的那一项。**