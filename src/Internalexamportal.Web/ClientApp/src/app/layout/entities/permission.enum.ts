export enum PermissionEnum {
    Dashboard = 1,
    QuestionManager = 2,
    SubjectManager = 3,
    TestManager = 4,
    CandidatesManager = 5,
    AdminManager = 6,

    //Dashboard
    ViewDashboard = 7,
    AddDashboard = 8,

    //Question
    ViewQuestion = 9,
    AddEditQuestion = 10,
    DeleteQuestion = 11,
    ImportQuestion = 12,

    //Subject 
    ViewSubject=13,
    AddEditSubject=14,
    DeleteSubject=15,

    //Test
    ViewTest = 16,
    AddEditTest=17,
    DeleteTest = 18,

    //Candidate
    ViewCandidate = 19,
    AddEditCandidate = 20,
    DeleteCandidate = 21,
    ImportCandidate = 22,

    //Admin
    ViewAdmin = 23,
    AddEditAdmin = 24,
    DeleteAdmin = 25,
    RolePermissions = 26            
}