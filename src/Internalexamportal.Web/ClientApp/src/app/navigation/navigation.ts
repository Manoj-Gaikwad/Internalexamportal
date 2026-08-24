import { FuseNavigation } from "@fuse/types";

export const navigation: FuseNavigation[] = [
    {
        id: 'main',
        title: 'Menu',
        translate: 'NAV.APPLICATIONS',
        type: 'group',
        icon: 'apps',
        children: [
            {
                id: 'dashboard',
                icon: 'dashboard',
                title: 'Dashboard',
                type: 'item',
                url: 'admin/dashboard',
            },
            {
                id: 'question',
                icon: 'question_answer',
                title: 'Question',
                type: 'collapsable',
                children: [
                    {
                        id: 'question-grid',
                        title: 'Question List',
                        type: 'item',
                        url: 'admin/question'
                    },
                    {
                        id: 'subject',
                        title: 'Subjects',
                        type: 'item',
                        url: 'admin/subject-details'
                    },
                    {
                        id: 'import-question',
                        title: 'Import Question',
                        type: 'item',
                        url: 'admin/import-question',
                        hidden: true
                    }
                ]
            },
            {
                id: 'test',
                icon: 'notes',
                title: 'Test Manager',
                type: 'collapsable',
                children: [
                    {
                        id: 'test-grid',
                        title: 'Test List',
                        type: 'item',
                        url: 'admin/tests'
                    },
                    {
                        id: 'add-test',
                        title: 'Add Test',
                        type: 'item',
                        url: 'admin/add-test'
                    },
                    {
                        id: 'test-instructions',
                        title: 'Test Instructions',
                        type: 'item',
                        url: 'admin/test-instructions'
                    }
                ]
            },
            {
                id: 'candidate',
                icon: 'people',
                title: 'Candidate',
                type: 'collapsable',
                children: [
                    {
                        id: 'candidate-list',
                        title: 'Candidate List',
                        type: 'item',
                        url: 'admin/candidate-list'
                    },
                    {
                        id: 'candidate-groups',
                        title: 'Candidate Groups',
                        type: 'item',
                        url: 'admin/candidate-groups'
                    }
                ]
            },
            {
                id: 'admin',
                icon: 'admin_panel_settings',
                title: 'Admin Settings',
                type: 'collapsable',
                children: [
                    {
                        id: 'manage-users',
                        title: 'Manage Users',
                        type: 'item',
                        url: 'admin/manage'
                    },
                    {
                        id: 'role-permission',
                        title: 'Role Permission',
                        type: 'item',
                        url: 'admin/permissions'
                    }
                ]
            },
            {
                id: 'client',
                icon: 'business',
                title: 'Manage Clients',
                type: 'item',
                url: 'admin/clients',
            }
        ]
    }
];

export const normalUserNavigation: FuseNavigation[] = [
    {
        id: 'main',
        title: 'Menu',
        translate: 'NAV.APPLICATIONS',
        type: 'group',
        icon: 'apps',
        children: [
            {
                id: 'cg-my-test',
                icon: 'dashboard',
                title: 'Home',
                type: 'item',
                url: 'candidates/my-test'
            },
            {
                id: 'cg-report',
                icon: 'list',
                title: 'Report',
                type: 'item',
                url: 'candidates/report'
            },
            {
                id: 'cg-upcoming-test',
                icon: 'event_note',
                title: 'Upcoming Tests',
                type: 'item',
                url: 'candidates/upcoming'
            }
        ]
    }
];
