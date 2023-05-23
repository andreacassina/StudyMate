namespace StudyMate.Services
{
    public interface IGreedyService
    {
        bool CalculateGreedy(int[] vettoreOreDisponibili, string userId, string userName, string degreeCourse, DateTime startingDate);
        Task deleteStudySlot(DateTime date, string userId);
    }
}
