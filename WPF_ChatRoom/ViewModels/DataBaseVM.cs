using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WPF_ChatRoom.Models;

namespace WPF_ChatRoom.ViewModels;

internal class DataBaseVM:ViewModelBase
{
    private ChatDbContext db;
    public ObservableCollection<ChatMessageEntity> UI_Messages { get; } = new ObservableCollection<ChatMessageEntity>();
    
    
    private ChatMessageEntity _selectedItem;
    public ChatMessageEntity SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            Selected();
        }
    }
    public string EndPoint { get; set; }
    public string Content { get; set; }
    
    
    public ICommand RefreshCommand { get; }
    public ICommand DeleteSelectCommand { get; }
    public ICommand SelectedCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    
    
    public DataBaseVM()
    {
        db=new ChatDbContext();
        RefreshCommand = new RelayCommand(_=>Refresh());
        DeleteSelectCommand =  new RelayCommand(_=>DeleteSelect());
        SelectedCommand = new RelayCommand(_=>Selected());
        AddCommand = new RelayCommand(_=>Add());
        EditCommand = new RelayCommand(_=>Edit());
        LoadMessages();
    }


    /// <summary>
    /// 执行刷新，获取最新的数据库内容
    /// </summary>
    private void Refresh()
    {
        UI_Messages.Clear();
        LoadMessages();
    }

    /// <summary>
    /// 删除选中行并保存
    /// </summary>
    private async Task DeleteSelect()
    {
        if (SelectedItem == null)return;
        var item = SelectedItem;
        UI_Messages.Remove(item);
        db.Message.Remove(item);
        await db.SaveChangesAsync();
    }
    
    /// <summary>
    /// 加载数据库内容到图表
    /// </summary>
    private async Task LoadMessages()
    {
        var list = await db.Message.OrderBy(x => x.Id).ToListAsync();
        foreach (var x in list)
        {
            UI_Messages.Add(x);
        }
    }



    /// <summary>
    /// 添加一行
    /// </summary>
    private async Task Add()
    {
        var Entity =  new ChatMessageEntity();
        Entity.SenderEndPoint = EndPoint;
        Entity.Content = Content;
        UI_Messages.Add(Entity);
        db.Message.Add(Entity);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// 选中后触发赋值到文本框
    /// </summary>
    private void Selected()
    {
        EndPoint = _selectedItem.SenderEndPoint;
        OnPropertyChanged(nameof(EndPoint));
        Content = _selectedItem.Content;
        OnPropertyChanged(nameof(Content));
    }

    /// <summary>
    /// 保存编辑当前项
    /// </summary>
    private async Task Edit()
    {
        _selectedItem.SenderEndPoint = EndPoint;
        OnPropertyChanged(nameof(EndPoint));
        _selectedItem.Content = Content;
        OnPropertyChanged(nameof(Content));
        await db.SaveChangesAsync();
    }
    
}