﻿using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Hero
{
    public class UpdateHeroDTO
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [Range(1, 1000)]
        public int? Rank { get; set; }

        public Guid? QuirkId { get; set; }

        [MaxLength(100)]
        public string? Affiliation { get; set; }

        public IFormFile? Image { get; set; }
    }
}