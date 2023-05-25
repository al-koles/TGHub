using System.ComponentModel.DataAnnotations;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Lotteries.Models;

public class LotteryButtonModel : IMapWith<PostButton>
{
    [Required] public string Content { get; set; } = null!;

    [Required] [Url] public string Link { get; set; } = null!;
}