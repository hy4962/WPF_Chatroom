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
    private void DeleteSelect()
    {
        UI_Messages.Remove(SelectedItem);
        SaveMessages();
    }
    
    /// <summary>
    /// 加载数据库内容到图表
    /// </summary>
    private void LoadMessages()
    {
        db.Message.OrderBy(x => x.Id).ToList().ForEach(x =>
        {
            UI_Messages.Add(x);
        });
    }

    /// <summary>
    /// 把当前表格内容覆盖数据里面的Message表格
    /// </summary>
    private void SaveMessages()
    {
        db.Message.ExecuteDelete();//删除整个Message表的内容
        foreach (var x in UI_Messages)
        {
            db.Message.Add(x);
        }
        db.SaveChanges();
    }


    /// <summary>
    /// 添加一行
    /// </summary>
    private void Add()
    {
        var Entity =  new ChatMessageEntity();
        Entity.SenderEndPoint = EndPoint;
        Entity.Content = Content;
        UI_Messages.Add(Entity);
        SaveMessages();
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
    private void Edit()
    {
        _selectedItem.SenderEndPoint = EndPoint;
        OnPropertyChanged(nameof(EndPoint));
        _selectedItem.Content = Content;
        OnPropertyChanged(nameof(Content));
        SaveMessages();
    }
    
}