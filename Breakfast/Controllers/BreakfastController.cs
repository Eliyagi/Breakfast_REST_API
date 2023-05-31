using ErrorOr;
using Microsoft.AspNetCore.Mvc;


public class BreakfastsController : ApiController
{
    //using the interface & dependency injection
    private readonly IBreafastService _breafastService;
    
    public BreakfastsController(IBreafastService breafastService) 
    {
        _breafastService = breafastService;
    }
    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        ErrorOr<Breakfast> requestToBreafastResult = Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet);

        if(requestToBreafastResult.IsError)
        {
            return Problem(requestToBreafastResult.Errors);
        }

        var breakfast = requestToBreafastResult.Value;

        //  save breakfast to database/memory
        ErrorOr<Created> CreateBreakfastResult = _breafastService.CreateBreakfast(breakfast);


        // Ability of the ErrorOr object to send a response or an error
        return CreateBreakfastResult.Match(
            Created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors));
    }

    

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breafastService.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors));
    }

    
    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        ErrorOr<Breakfast> requestToBreafastResult =  Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime, 
            request.Savory,
            request.Sweet,
            id);
        
        if(requestToBreafastResult.IsError)
        {
            return Problem(requestToBreafastResult.Errors);
        }
        var breakfast = requestToBreafastResult.Value;
        ErrorOr<UpsertedBreakfast> UpsertedBreakfastResult = _breafastService.UpsertBreakfast(breakfast);
        

        //TO DO: return 201 if a new breafast was created
        return UpsertedBreakfastResult.Match(
            upserted => upserted.isNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deleteBreakfastResults = _breafastService.DeleteBreakfast(id);
        return deleteBreakfastResults.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }



    private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastResponse(breakfast));
    }
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDateTime,
                    breakfast.Savory,
                    breakfast.Sweet);
    }

}