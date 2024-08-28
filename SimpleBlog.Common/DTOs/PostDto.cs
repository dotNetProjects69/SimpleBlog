﻿namespace SimpleBlog.Common.DTOs;

public class PostDto
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public AccountDto? Owner { get; set; }
}