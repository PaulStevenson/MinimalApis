namespace MinimalApiDemo.Validation
{
    public class Validators : AbstractValidator<Article>
    {
        public Validators()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(10)
                .NotEmpty();
            RuleFor(x => x.Content)
                .MaximumLength(255).NotEmpty()
                .WithMessage("You silly billy")
                .WithName("Content");
            RuleFor(x => x.MyNumber)
                .GreaterThan(0);
        }
    }

    public class ArticleRequestValidator : AbstractValidator<ArticleRequest>
    {
        public ArticleRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(10)
                .NotEmpty();
            RuleFor(x => x.Content)
                .MaximumLength(255).NotEmpty()
                .WithMessage("You silly billy")
                .WithName("Content");
            RuleFor(x => x.MyNumber)
                .GreaterThan(0);
        }
    }
}
