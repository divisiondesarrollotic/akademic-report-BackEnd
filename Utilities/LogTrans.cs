using AkademicReport.Data;
using AkademicReport.Models;

namespace AkademicReport.Utilities
{
    public static class LogTrans
    {
       
        public static async Task<bool> SaveLogTransaction(LogTransacional log)
        {
            try
            {
                using (var dataContext = new DataContext())
                {
                    dataContext.LogTransacionals.Add(log);
                    await dataContext.SaveChangesAsync();  
                    return true;
                }
            }
            catch (Exception ex)
            {

              return false;
            }
           
        }
    }
}
