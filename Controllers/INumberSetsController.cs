using LottoGeneratorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace LottoGeneratorService.Controllers;
public interface INumberSetsController
{
    Task<IActionResult> GetAsync(SetsRequest setsRequest);
}