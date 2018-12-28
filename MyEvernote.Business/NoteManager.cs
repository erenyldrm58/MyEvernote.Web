using MyEvernote.Business.Abstract;
using MyEvernote.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyEvernote.Business
{
    public class NoteManager : ManagerBase<Note>
    {
        public List<Note> GetAllNotes()
        {
            return List();
        }

        public IQueryable<Note> GetAllNoteQueryable()
        {
            return ListQueryable();
        }
    }
}
