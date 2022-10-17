using System;
using BuberBreakfast.Contracts;
using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("breakfast")]
public class BreakfastController:ControllerBase
{
	private readonly IBreakfastService _breakfastService;

	public BreakfastController(IBreakfastService breakfastService){
		_breakfastService = breakfastService;
	}

	[HttpPost("/breakfasts")]
	public IActionResult CreateBreakfast(CreateBreakfastRequest request)
	{
		var breakfast = new Breakfast(
			Guid.NewGuid(),
			request.Name,
			request.Description,
			request.StartDateTime,
			request.EndDateTime,
			DateTime.UtcNow,
			request.Savory,
			request.Sweet
		);

		// TODO: save breakfast to database
		_breakfastService.CreateBreakfast(breakfast);

		var response = new BreakfastResponse(
			breakfast.Id,
			breakfast.Name,
			breakfast.Description,
			breakfast.StartDateTime,
			breakfast.EndDateTime,
			breakfast.LastModifiedDateTime,
			breakfast.Savory,
			breakfast.Sweet
		);

		return CreatedAtAction(nameof(GetBreakfast), new{id = breakfast.Id}, response);
	}

	[HttpGet("{id:guid}")]
	public IActionResult GetBreakfast(Guid id)
	{
		Breakfast breakfast = _breakfastService.GetBreakfast(id);

		var response = new BreakfastResponse(
			breakfast.Id,
			breakfast.Name,
			breakfast.Description,
			breakfast.StartDateTime,
			breakfast.EndDateTime,
			breakfast.LastModifiedDateTime,
			breakfast.Savory,
			breakfast.Sweet
		);
		return Ok(response);
	}

	[HttpPut("/breakfasts/{id:guid}")]
	public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
	{
		var breakfast  = new Breakfast(
			id,
			request.Name,
			request.Description,
			request.StartDateTime,
			request.EndDateTime,
			DateTime.UtcNow,
			request.Savory,
			request.Sweet
		);

		_breakfastService.UpsertBreakfast(breakfast);
		return NoContent();
	}

	[HttpDelete("/breakfasts/{id:guid}")]
	public IActionResult DeleteBreakfast(Guid id)
	{
		_breakfastService.DeleteBreakfast(id);
		return Ok(id);
	}
}
