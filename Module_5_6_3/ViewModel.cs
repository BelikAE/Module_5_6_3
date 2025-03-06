using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Module_5_6_3
{
    public class ViewModel : BaseViewModel
    {
        public event EventHandler CloseRequest;
        private ExternalCommandData _commandData;
        public DelegateCommand CreateCommand { get; }
        public List<FamilySymbol> Types { get; } = new List<FamilySymbol>();
        public FamilySymbol SelectedType { get; set; }
        public List<XYZ> Points { get; set; } = new List<XYZ>();
        public int Amount { get; set; }



        public ViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            CreateCommand = new DelegateCommand(OnCreateCommand);
            Points = SelectionUtils.Get2Points(_commandData, "Выберите начальную и конечную точку", ObjectSnapTypes.Endpoints);
            Types = FamilySymbolUtils.GetFamilySymbols(_commandData);
            Amount = 2;
        }

        private void OnCreateCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FamilyInstanceUtils.CreateElementsAlongLine(doc, Points[0], Points[1], SelectedType, Amount);

            RaiseCloseRequest();
        }

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

    }
}
