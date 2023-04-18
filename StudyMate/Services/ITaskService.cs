namespace StudyMate.Services
{
    public interface ITaskService
    {
        StudyMate.Models.Event GetTaskById(int id);
        IEnumerable<StudyMate.Models.EventViewModel> GetAllTasks();
        IEnumerable<StudyMate.Models.EventViewModel> GetAllLessons();
        IEnumerable<StudyMate.Models.EventViewModel> GetAllPersonalEvents();
        StudyMate.Models.Event AddTask(StudyMate.Models.Event task);
    }
}
