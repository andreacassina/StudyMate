namespace StudyMate.Services
{
    public interface ITaskService
    {
        StudyMate.Models.Event GetTaskById(int id);
        IEnumerable<StudyMate.Models.EventViewModel> GetAllTasks();
        IEnumerable<StudyMate.Models.EventViewModel> GetAllLessons();
        IEnumerable<StudyMate.Models.EventViewModel> GetAllPersonalEvents();
        StudyMate.Models.Event AddTask(StudyMate.Models.Event task);

        int GetFreeHours(DateTime date);                            //Ricerco il tempo libero che ho a disposizione nella data specificata incrociando gli eventi personali e gli orari delle lezioni
                                                                    //suppongo orari non calcolabili gli orari: 
                                                                    // - tra le 00:00 e le 08:00 (sonno)
                                                                    // - tra le 12:00 e le 14:00 (cibo)
                                                                    // - tra le 19:00 e le 21:00 (cena)
    }
}
