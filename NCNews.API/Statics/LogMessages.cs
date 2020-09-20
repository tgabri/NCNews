using NCNews.API.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.Statics
{
    public static class LogMessages
    {
        public static string AttemptedToGet(string location) => $"{location}: Attempted to get all";
        public static string AttemptedToGet(string location, int id) => $"{location}/{id}: Attempted to get";
        public static string AttemptedToCreate(string location) => $"{location}: Attempted to create";
        public static string AttemptedToUpdate(string location, int id) => $"{location}/{id}: Attempted to update";
        public static string AttemptedToDelete(string location, int id) => $"{location}/{id}: Attempted to delete";

        public static string Success(string location) => $"{location}: Successful";
        public static string Success(string location, int id) => $"{location}/{id}: Successful";

        public static string InternalError(string location, string message, Exception innerException) => $"{location}: {message} - {innerException}";
        public static string InternalError(string location, int id, string message, Exception innerException) => $"{location}/{id}: {message} - {innerException}";

        public static string NotFound(string location, int id) => $"{location}/{id}: Not found";
        public static string BadData(string location, int id) => $"{location}/{id}: Failed with bad data";
        public static string IncompleteData(string location) => $"{location}: Data was incomplete";
        public static string IncompleteData(string location, int id) => $"{location}/{id}: Data was incomplete";

        public static string EmptyRequest(string location) => $"{location}: Empty request was submitted";

        public static string CreateFailed(string location) => $"{location}: Creation failed";

        public static string UpdateFailed(string location, int id) => $"{location}/{id}: Update failed";

        public static string DeleteFailed(string location, int id) => $"{location}/{id}: Delete failed";
    }
}
