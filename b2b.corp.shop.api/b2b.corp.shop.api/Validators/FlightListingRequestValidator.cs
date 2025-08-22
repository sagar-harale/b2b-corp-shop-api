using b2b.corp.shop.api.Models.Api;
using FluentValidation;

namespace Tzy.Flight.Api.Validators
{
    public class FlightListingRequestValidator : AbstractValidator<FlightListingApiRequest>
    {
        public FlightListingRequestValidator()
        {
            RuleFor(x => x.TripType.ToUpper())
                .NotEmpty().WithMessage("TripType is required.")
                .Must(t => t == "ONE_WAY" || t == "ROUND_TRIP").WithMessage("TripType must be ONE_WAY or ROUND_TRIP.");

            RuleFor(x => x.From)
                .NotEmpty().WithMessage("Origin (From) is required.")
                .Length(3, 3).WithMessage("Origin must be a 3-letter airport code.");

            RuleFor(x => x.To)
                .NotEmpty().WithMessage("Destination (To) is required.")
                .Length(3, 3).WithMessage("Destination must be a 3-letter airport code.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Date must be in YYYY-MM-DD format.");

            RuleFor(x => x.ReturnDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.TripType == "ROUND_TRIP").WithMessage("ReturnDate is required for round trips.")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").When(x => x.TripType == "ROUND_TRIP" && !string.IsNullOrEmpty(x.ReturnDate)).WithMessage("ReturnDate must be in YYYY-MM-DD format.");

            RuleFor(x => x.Class.ToUpper())
                .NotEmpty().WithMessage("Class is required.")
                .Must(c => c == "ECONOMY" || c == "BUSINESS" || c == "FIRST").WithMessage("Class must be ECONOMY, BUSINESS, or FIRST.");

            RuleFor(x => x.Adults)
                .GreaterThan(0).WithMessage("At least one adult is required.");

            RuleFor(x => x.Children)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Infants)
                .GreaterThanOrEqualTo(0);
        }
    }
}
