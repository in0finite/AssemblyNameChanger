using System;
using Gtk;
using Mono.Cecil;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton3Clicked (object sender, EventArgs e)
	{

		try {
			
			if (this.filechooserbutton2.Filename.Length < 1) {
				throw new Exception ("Assembly not selected");
			}

			if (this.filechooserbutton3.Filename.Length < 1) {
				throw new Exception ("Output file not selected");
			}

			if (this.entry1.Text.Length < 1) {
				throw new Exception ("New name is not assigned");
			}


			var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(this.filechooserbutton2.Filename);

			assemblyDefinition.Name.Name = this.entry1.Text;
			assemblyDefinition.MainModule.Name = this.entry1.Text;

			// Build the new assembly
			assemblyDefinition.Write(this.filechooserbutton3.Filename, new Mono.Cecil.WriterParameters() { WriteSymbols = true });


			var dialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "Assembly modified");
			dialog.Show();

		} catch( Exception ex ) {
			var dialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, ex.ToString());
			dialog.Show ();
		}


	}

	protected void OnFilechooserbutton2SelectionChanged (object sender, EventArgs e)
	{

		var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(this.filechooserbutton2.Filename);

		string text = "Assembly loaded \n\n";
		text += assemblyDefinition.Name + "\n\n";
	//	text += assemblyDefinition.ToString () + "\n";
		foreach (var r in assemblyDefinition.MainModule.AssemblyReferences) {
			text += r.ToString () + "\n";
		}
		text += "\n";
		foreach (var t in assemblyDefinition.MainModule.GetTypes()) {
			text += t.ToString () + "\n";
		}

//		var dialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, text);
//		dialog.Show();

		this.textview2.Buffer.Text = text;

	}
}
