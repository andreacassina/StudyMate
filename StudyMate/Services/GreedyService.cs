using StudyMate.Models;

namespace StudyMate.Services
{
    public class GreedyService : IGreedyService
    {
        private readonly PACContext _context;
        //private readonly ApplicationUser _users;
        public GreedyService(PACContext context)
        {
            _context = context;
        }

        private Dictionary<int, int> SearchFreeHours(int[] vettore)
        {
            var dizionario = new Dictionary<int, int>();
            int lunghezza = vettore.Length;
            int posizione = -1;
            int count = 0;

            for (int i = 0; i < lunghezza; i++)
            {
                if (vettore[i] == 1)
                {
                    if (posizione == -1)
                    {
                        posizione = i;
                    }
                    count++;
                }
                else if (count > 0)
                {
                    dizionario[posizione] = count;
                    posizione = -1;
                    count = 0;
                }
            }

            // Se l'ultimo elemento del vettore è un 1, aggiungilo al dizionario
            if (count > 0)
            {
                dizionario[posizione] = count;
            }

            return dizionario;
        }

        private List<StudySlot> SearchCandidates(string degreeCourse, string userName, string userId)
        {
            var courses = _context.Courses.Where(c => c.DegreeCourse.Equals(degreeCourse)).Select(c => new
            {
                ProfessorName = c.ProfessorName,
                CourseName = c.CourseName,
                DegreeCourse = c.DegreeCourse,
                CourseId = c.CourseId,
                Cfu = Math.Ceiling((((c.Cfu * 25) - 100) * 1.1) / 12)       // ore da studiare nella settimana
            });

            // Sottrazione ore già studiate --> Da aggiungere

            List<StudySlot> studySlots = new List<StudySlot>();

            foreach (var course in courses)
            {
                double threeHoursSlots = Math.Floor(course.Cfu / 3);      // ore di slot da 3
                double oneHourSlots = course.Cfu - 3 * threeHoursSlots;                // slot da 1

                DateTime deadline = _context.Deadlines.Where(d => d.CourseId.Equals(course.CourseId)).Select(d => d.ExamDate).FirstOrDefault();     // deadline del corso
                double remainingDays = Math.Floor((deadline - DateTime.Today).TotalDays);   // Giorni rimanenti rispetto alla data di oggi

                double priority = (1 / remainingDays) + (((course.Cfu * 25) - 100) * 1.1) ;        // Priorità

                for(int i = 0; i < threeHoursSlots; i++)
                {
                    StudySlot sl = new StudySlot();
                    sl.CourseId = course.CourseId;
                    sl.CourseName = course.CourseName;
                    sl.Priority = priority;
                    sl.Hours = 3;
                    sl.UserName = userName;
                    sl.UserId = userId;

                    studySlots.Add(sl);
                }

                for (int i = 0; i < oneHourSlots; i++)
                {
                    StudySlot sl = new StudySlot();
                    sl.CourseId = course.CourseId;
                    sl.CourseName = course.CourseName;
                    sl.Priority = priority;
                    sl.Hours = 1;
                    sl.UserName = userName;
                    sl.UserId = userId;
                    studySlots.Add(sl);

                }

            }
            Shuffle(studySlots);
            studySlots.OrderByDescending(s => s.Hours);

            return studySlots;
        }

        private void Shuffle<StudySlot>(List<StudySlot> list)
        {
            Random random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                StudySlot value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public bool CalculateGreedy(int[] vettoreOreDisponibili, string userId, string userName, string degreeCourse, DateTime startingDate)
        {
            List<StudySlot> candidates = SearchCandidates(degreeCourse, userName, userId);      // Elenco dei canditati
            Dictionary<int, int> freeHours = SearchFreeHours(vettoreOreDisponibili);

            List<StudySlot> solution = new List<StudySlot>();       // Inizializzo la lista vuota

            while(!Ottimo(freeHours) && candidates.Count != 0)
            {
                StudySlot? sl = candidates.FirstOrDefault();
                candidates.RemoveAt(0);
                if (Ammissibile(sl, freeHours, startingDate))
                    solution.Add(sl);
            }
            if (Ottimo(freeHours))
                return true;
            else
                return false;

        }

        private bool Ottimo(Dictionary<int, int> freeHours)
        {
            int sum = 0;
            foreach (var kvp in freeHours)
            {
                sum += kvp.Value;
            }

            if (sum == 0)
                return true;
            else
                return false;
        }

        private bool Ammissibile(StudySlot sl, Dictionary<int, int> freeHours, DateTime startingDate)
        {
            foreach(var kvp in freeHours)
            {
                if(kvp.Value >= sl.Hours)
                {
                    freeHours[kvp.Key] = kvp.Value - sl.Hours;      // Aggiorno le ore disponibili nel dizionario
                    // Compilazione valori orari il sl --> parto dal fondo
                    sl.From = startingDate.AddDays(kvp.Key % 24);
                    TimeSpan ts = new TimeSpan(kvp.Key%24 + kvp.Value - sl.Hours, 0, 0);
                    sl.From = sl.From.Date + ts;
                    //sl.From.AddHours(kvp.Key + kvp.Value - sl.Hours);

                    sl.To = startingDate.AddDays(kvp.Key % 24);
                    ts = new TimeSpan(kvp.Key % 24 + kvp.Value, 0, 0);
                    sl.To = sl.To.Date + ts;
                    //sl.To.AddHours(kvp.Key + kvp.Value);

                    //Inserimento Study slot nel database
                    _context.StudySlots.Add(sl);
                    _context.SaveChanges();

                    return true;
                }
            }
            return false;
            
        }


    }
}
