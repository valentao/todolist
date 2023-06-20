using FluentValidation;
using TodolistMvc.Models.Tasks;

namespace TodolistMvc.Models.Validators
{
    public class TaskEditValidator : AbstractValidator<TaskEditDTO>
    {
        public TaskEditValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Length(5, 100).WithMessage("Name should have from 5 to 100 characters");
            RuleFor(x => x.TaskPriorityId).NotNull();

        }
    }
}
