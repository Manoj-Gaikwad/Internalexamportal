
export class CandidateModel {
    DateOfBirth: string;
    Email: string;
    FirstName: string;
    Gender: string;
    LastName: string;
    Phone: string;
    isValid: boolean;
}

export class SaveRolePermissionModel {
    public roleId: string;
    rolePermissions: RolePermissionModel[] = [];
}

export class RolePermissionModel {
    public id: number;
    public parentId: number;
    public permissionName: string;
    public isActive: boolean;

    //RolePermission Properties
    public isChecked: boolean;
}

export class TestQuestion {
    public id: number;
    public testId: number;
    public questionId: number;
    public positiveMark: number;
    public negativeMark: number;
}

export class Question {
    public id: number;
    public description: string;
    public subjectId: number;
    public subjectTopicId: number;
    public questionTypeId: number;
    public questionOption: Array<QuestionOption>;
    public questionMark: QuestionMark;
}

export class QuestionOption {
    public id: number;
    public optionText: string;
    public questionId: number;
    public isAnswer: boolean;
}

export class QuestionMark {
    public id: number;
    public rightMark: string;
    public negativeMark: string;
    public difficultLevel: string;
    public questionId: number;
}

export class ExamQuestion {
    public id: number;
    public testId: number;
    public questionId: number;
    public positiveMark: number;
    public negativeMark: number;
    public isMultiSelect: boolean;
    public question: QuestionModel;
    public answered: boolean;
    public NotAnswered: boolean;
    public Marked: boolean;
    public timeTaken: number = 0;
}

export class QuestionModel {
    public id: number;
    public description: string;
    public questionTypeId: number;
    public questionOption: Array<QuestionOptionModel>;
}

export class QuestionOptionModel {
    public id: number;
    public optionText: string;
    public questionId: number;
    public isChecked: boolean;
}

export class SubjectiveAnswer {
    public questionId: number;
    public answerText: string;
    public timeTaken: number;
}

export class SubmittedTest {
    public id: number;
    public submitDate: Date;
    public obtainedMark: number;
    public rightMark: number;
    public negativeMark: number;
    public totalMark: number;
    public userName: string;
    public testName: string;
    public objective: Array<ObjectiveQuestion>;
    public subjective: Array<SubjectiveQuestion>;
}

export class ObjectiveQuestion {
    public id: number;
    public questionType: number;
    public question: string;
    public questionOptions: Array<QuestionOption>;
    public selectedOptions: Array<number>;
    public positiveMarks: number;
    public negativeMarks: number;
}

export class SubjectiveQuestion {
    public id: number;
    public questionType: number;
    public question: string;
    public questionOption: QuestionOption;
    public submittedAnswer: string;
    public positiveMarks: number;
    public negativeMarks: number;
    public obtainedMarks?: number;
    public comment: string;
}

export class Client {
    public id: number;
    public name: string;
    public email: string;
    public phone: string;
    public address: string;
    public logo: string;
}