namespace MoncayoLoaizaJI_TareaMVVM.Views;

public partial class AllNotesPageJorgeM : ContentPage
{
	public AllNotesPageJorgeM()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}