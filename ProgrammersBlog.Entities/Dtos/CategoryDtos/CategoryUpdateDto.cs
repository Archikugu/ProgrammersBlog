﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.CategoryDtos;

public class CategoryUpdateDto
{
    [Required]
    public int Id { get; set; }

    [DisplayName("Category Name")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(70, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters")]
    public string Name { get; set; }

    [DisplayName("Category Description")]
    [MaxLength(500, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters")]
    public string Description { get; set; }

    [DisplayName("Category Note")]
    [MaxLength(500, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters")]
    public string Note { get; set; }

    [DisplayName("Category Is Active ?")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public bool IsActive { get; set; }

    [DisplayName("Category Is Deleted ?")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public bool IsDeleted { get; set; }
}

