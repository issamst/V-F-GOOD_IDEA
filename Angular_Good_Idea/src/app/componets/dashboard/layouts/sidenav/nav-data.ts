import { INavbarData } from './helper';

export const navbarData: INavbarData[] = [
  {
    routeLink: 'proposeidea',
    icon: 'fa fa-lightbulb',
    label: 'Propose Idea',
    role: 'Null,Admin,Team Leader,Committee,User,Operator'
  },
  {
    routeLink: 'managerresources',
    icon: 'fa fa-users',
    label: 'Manage Resources',
    role: 'Admin',
    expanded: false,
    items: [
      { routeLink: 'managerresources/users', label: 'Users' },
      { routeLink: 'managerresources/team-leader', label: 'Team Leaders' },
      { routeLink: 'managerresources/committees', label: 'Committees' },
    
    ]
  },
  {
    routeLink: 'masterdata',
    icon: 'fa fa-database',
    label: 'Master Data',
    role: 'Admin',
    expanded: true,
    items: [
      { routeLink: 'masterdata/plants', label: 'Plants' },
      { routeLink: 'masterdata/departement', label: 'Departements' },
      { routeLink: 'masterdata/areas', label: 'Areas' },
      { routeLink: 'masterdata/project', label: 'Projects' },
      { routeLink: 'masterdata/machines', label: 'Machines' },
      { routeLink: 'masterdata/roles', label: 'Roles' },
      { routeLink: 'masterdata/titles', label: 'Titles' },
      { routeLink: 'masterdata/impact', label: 'Impacts' }
    ]
  },
  {
    routeLink: 'Approval_committee',
    icon: 'fa fa-check-circle',
    label: 'Approval Committee',
    role: 'Admin,Committee',
    expanded: true,
    items: [
      { routeLink: 'Approval_committee/Committee', label: 'Committee' }
    ]
  },
  {
    routeLink: 'Approval_teamleader',
    icon: 'fa fa-check-circle',
    label: "My Team's Ideas",
    role: 'Admin,Team Leader',
    expanded: true,
    items: [
      { routeLink: 'Approval_teamleader/Team_Leader', label: "My Team's Ideas" }
    ]
  }
];