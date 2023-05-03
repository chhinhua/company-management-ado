using System;
using System.Collections.Generic;
using company_management.BUS;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;

namespace company_management.View.UC
{
    public partial class UcTeam : UserControl
    {
        private readonly Lazy<TeamBus> _teamBus;

        public UcTeam()
        {
            _teamBus = new Lazy<TeamBus>(() => new TeamBus());
            InitializeComponent();
        }

        private void UC_Team_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            LoadDataGridview(_teamBus.Value.GetListTeamByPosition());
        }
        
        private void LoadDataGridview(List<Team> teams)
        {
            _teamBus.Value.LoadDataGridview(teams, dataGridView_Team);
        }

    }
}
