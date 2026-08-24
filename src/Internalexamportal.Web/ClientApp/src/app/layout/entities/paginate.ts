
export class Paginate {
    pageNumber: number = 1;
    pageSize: number = 10;
    searchTerm: string = null;
    totalRecords: number;
    sortingColumn: string;
    sortingDirection: string;
}

export class QuestionPaginate {
    pageNumber: number = 1;
    pageSize: number = 10;
    searchTerm: string = null;
    totalRecords: number;
    sortingColumn: string;
    sortingDirection: string;
    subjectIds: Array<number>;
    subjectTopicIds: Array<number>;
    questionTypeIds: Array<number>;
}

export class CandidatePaginate {
    pageNumber: number = 1;
    pageSize: number = 10;
    searchTerm: string = null;
    groups: Array<number> = [];
    totalRecords: number;
    sortingColumn: string;
    sortingDirection: string;
}

export class SubmittedTestPaginate {
    testId: number;
    pageNumber: number = 1;
    pageSize: number = 10;
    searchTerm: string = null;
    totalRecords: number;
    sortingColumn: string;
    sortingDirection: string;
}