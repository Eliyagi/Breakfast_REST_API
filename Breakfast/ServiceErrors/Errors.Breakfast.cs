using ErrorOr;

public static class Errors
{
    public static class BreakfastError
    {
        public static Error InvalidName => Error.Validation (
            code: "Breakfast.InvalidName",
            description:$"Breakfast name must be at least {Breakfast.MinNameLength}"+
             $" characters long and at most {Breakfast.MaxNameLength} characters long ");

         public static Error InvalidDescription => Error.Validation (
            code: "Breakfast.InvalidDescription",
            description:$"Breakfast description must be at least {Breakfast.MinDescriptionLength}"+
             $" characters long and at most {Breakfast.MaxDescriptionLength} characters long ");

       public static Error NotFound => Error.NotFound (
            code: "Breakfast.NotFound",
            description: "Breakfast not fuond");
    } 
}