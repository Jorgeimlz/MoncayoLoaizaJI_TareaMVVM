using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MoncayoLoaizaJI_TareaMVVM.Models;

namespace MoncayoLoaizaJI_TareaMVVM.ViewModelsJorgeM
{
    internal class NotesViewModelJorgeM : IQueryAttributable
    {
        public ObservableCollection<ViewModelsJorgeM.NoteViewModelJorgeM> AllNotes { get; set; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        public NotesViewModelJorgeM() {
            AllNotes = new ObservableCollection<ViewModelsJorgeM.NoteViewModelJorgeM>(Models.Note.LoadAll().Select(n => new NoteViewModelJorgeM(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<ViewModelsJorgeM.NoteViewModelJorgeM>(SelectNoteAsync);
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.NotePageJorgeM));
        }

        private async Task SelectNoteAsync(ViewModelsJorgeM.NoteViewModelJorgeM note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(Views.NotePageJorgeM)}?load={note.Identifier}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                NoteViewModelJorgeM matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note exists, delete it
                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                NoteViewModelJorgeM matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note is found, update it
                if (matchedNote != null)
                {
                    matchedNote.Reload();
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                }
                // If note isn't found, it's new; add it.
                else
                    AllNotes.Insert(0, new NoteViewModelJorgeM(Models.Note.Load(noteId)));
            }
        }



    }
}
