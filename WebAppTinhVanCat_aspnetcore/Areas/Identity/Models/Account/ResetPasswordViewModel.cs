// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTinhVanCat_aspnetcore.Areas.Identity.Models.AccountViewModels
{

    public class ResetPasswordViewModel
    {
            [Required(ErrorMessage = "Phải nhập {0}")]
            [EmailAddress(ErrorMessage="Phải đúng định dạng email")]
            [Display(Name = "Email của tài khoản", Prompt = "Email của tài khoản")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(100, ErrorMessage = "{0} phải từ {2} đến {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nhập mật khẩu mới", Prompt = "Nhập mật khẩu mới")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Lặp lại mật khẩu", Prompt = "Lặp lại mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu lập lại không khớp.")]
            public string ConfirmPassword { get; set; }

            [AllowNull]      
            public string Code { get; set; }

    }
}
