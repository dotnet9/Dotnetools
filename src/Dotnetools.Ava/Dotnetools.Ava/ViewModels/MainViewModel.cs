﻿using GTranslate.Translators;
using ReactiveUI;
using Slugify;
using System;

namespace Dotnetools.Ava.ViewModels;

/// <summary>
/// 中文转英文，英文转别名
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly GoogleTranslator2 _bingTranslator = new();
    private readonly SlugHelper _slugHelper = new();

    private string? _chinese;

    /// <summary>
    /// 中文标题
    /// </summary>
    public string? Chinese
    {
        get => _chinese;
        set
        {
            if (value != _chinese)
            {
                this.RaiseAndSetIfChanged(ref _chinese, value);
            }
        }
    }

    private string? _english;

    /// <summary>
    /// 英语标题
    /// </summary>
    public string? English
    {
        get => _english;
        set
        {
            if (value != _english)
            {
                this.RaiseAndSetIfChanged(ref _english, value);
            }
        }
    }

    private string? _slug;

    /// <summary>
    /// 别名
    /// </summary>
    public string? Slug
    {
        get => _slug;
        set
        {
            if (value != _slug)
            {
                this.RaiseAndSetIfChanged(ref _slug, value);
            }
        }
    }

    public MainViewModel()
    {
        this.WhenAnyValue(x => x.Chinese)
            .Subscribe(HandleChangeChineseCommand);
        this.WhenAnyValue(x => x.English)
            .Subscribe(HandleChangeEnglishCommand);
    }

    /// <summary>
    /// 将中文转为英文
    /// </summary>
    private async void HandleChangeChineseCommand(string? chinese)
    {
        if (string.IsNullOrWhiteSpace(chinese))
        {
            English = string.Empty;
            return;
        }

        try
        {
            English = (await _bingTranslator.TranslateAsync(chinese, "en")).Translation;
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    /// <summary>
    /// 将英文转为URL友好格式
    /// </summary>
    /// <param name="english"></param>
    private async void HandleChangeEnglishCommand(string? english)
    {
        if (string.IsNullOrWhiteSpace(english))
        {
            Slug = string.Empty;
            return;
        }

        try
        {
            Slug = _slugHelper.GenerateSlug(english);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}