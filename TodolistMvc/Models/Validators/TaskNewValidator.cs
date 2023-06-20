using FluentValidation;
using TodolistMvc.Models.Tasks;

namespace TodolistMvc.Models.Validators
{
    public class TaskNewValidator : AbstractValidator<TaskNewDTO>
    {
        public TaskNewValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Length(5, 100).WithMessage("Name should have from 5 to 100 characters");
            RuleFor(x => x.TaskPriorityId).NotNull();

        }
    }
}
