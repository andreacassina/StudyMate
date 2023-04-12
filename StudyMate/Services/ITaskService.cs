namespace StudyMate.Services
{
    public interface ITaskService
    {
        StudyMate.Models.Event GetTaskById(int id);
        IEnumerable<StudyMate.Models.Event> GetAllTasks();
        StudyMate.Models.Event AddTask(StudyMate.Models.Event task);
    }
}
