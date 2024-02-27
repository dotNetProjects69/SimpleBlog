﻿using System;
using System.Collections.Generic;

namespace SimpleBlog.EFCore;

public partial class AuthData
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? UserId { get; set; }

    public string? NickName { get; set; }
}
