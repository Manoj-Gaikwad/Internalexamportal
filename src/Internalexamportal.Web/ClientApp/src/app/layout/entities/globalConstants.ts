import { HttpEventType } from '@angular/common/http';

export const emailPattern = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
export const namePattern = new RegExp(/([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$/)
export const inputNamePattern = new RegExp(/^[a-zA-Z .-]*$/);
export const phoneNumberPattern = new RegExp(/^[789]\d{9}$/);
export const numberPattern = new RegExp('^[0-9]*$');
export const numDecimalPattern = /^[0-9]+\.?[0-9]*$/;
export const genderPattern = new RegExp(/^(?:m|M|male|Male|MALE|f|F|female|Female|FEMALE)$/);
export const datePattern = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/);
export const expirationDatePattern = new RegExp('^([0-9]{1,2})[/]+([0-9]{2}|[0-9]{4})$')
export const upperCasePattern = /[A-Z]/;
export const lowerCasePattern = /[a-z]/;
export const numbersPattern = /\d/;
export const specialPattern = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
export const decimalPattern = new RegExp('^[0-9.]*$');
export const timePattern = new RegExp(/\b((1[0-2]|0?[1-9]):([0-5][0-9]) ([AaPp][Mm]))/);
export const passwordPattern = new RegExp('(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@#$!%*?&-])[A-Za-z\\d$@#$!%*?&-]{8,50}');

export const minBirthDate = new Date(1900,0,2);

export enum Roles {
    SuperAdmin = "SuperAdmin",
    Admin = "Admin",
    Candidate = "Candidate",
}

export enum TestSetting {
    Shuffling = 1,
    DisplayResult = 2,
    MultipleAttempts = 3,
    WindowMinimiseWarning = 4,
    DisplayAnswerSheet = 5
}

export enum QuestionType {
    MultipleChoice = 1,
    TrueFalse = 2,
    Subjective = 3
}

export enum TestType {
    Objective = 1,
    Subjective = 2,
    ObjectiveAndSubjective = 3
}

export enum TestStatus {
    InProgress = 1,
    Submitted = 2,
    Evaluated = 3
}

export const fuseFullScreenConfig = {
    layout: {
        navbar: {
            hidden: true,
        },
        toolbar: {
            hidden: true,
        },
        footer: {
            hidden: true,
        },
        sidepanel: {
            hidden: true,
        },
    },
};

export const maxCSVFileSizeInMegaBytes = 1;

export const candidateImportCSVFilePath = "/assets/data/candidate_import.csv";

export const cryptoKey = "E@H+MbQeThWmZq4";

export const DATE_FORMAT = {
    parse: {
        dateInput: "DD/MM/YYYY",
    },
    display: {
        dateInput: "DD/MM/YYYY",
        monthYearLabel: "DD/MM/YYYY",
        dateA11yLabel: "LL",
        monthYearA11yLabel: "YYYY",
    },
};

export const exportCSVOptions = {
    filename: 'Candiate_Export_',
    fieldSeparator: ',',
    quoteStrings: '"',
    decimalSeparator: '.',
    showLabels: true,
    showTitle: false,
    title: 'Candidates List',
    useTextFile: false,
    useBom: true,
    // useKeysAsHeaders: true,
    headers: ['First Name', 'Last Name', 'Email', 'Phone Number', 'Group', 'Registration Date']
};

// Quill editor configuration
export const quillConfig = {
    modules: {
        toolbar: [
            ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
            ['blockquote', 'code-block'],
            [{ 'header': 1 }, { 'header': 2 }],               // custom button values
            [{ 'list': 'ordered'}, { 'list': 'bullet' }],
            [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
            [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
            [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
            [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults
            [{ 'font': [] }],
            [{ 'align': [] }],
            ['clean'],                                        // remove formatting button
            ['link','image','video']                        // media
        ]
    },
    theme: 'snow'  // or 'bubble'
};

export const maxTestWarningCount = 3;

export function getFileExtension(docType) {
    switch (docType) {
        case "text/plain": return ".txt";
        case "application/pdf": return ".pdf";
        case "application/vnd.ms-word": return ".doc";
        // case "application/vnd.ms-word": return ".docx";
        case "application/vnd.ms-excel": return ".xls";
        case "application/octet-stream": return ".xlsx";
        case "image/png": return ".png";
        case "image/jpeg": return ".jpg";
        // case "image/jpeg": return ".jpeg";
        case "image/gif": return ".gif";
        case "text/csv": return ".csv";
        default: return ".pdf";
    }
}


export function downloadFile(data, fileName: string) {
    switch (data.type) {
        case HttpEventType.DownloadProgress:
            break;
        case HttpEventType.Response:
            const downloadedFile = new Blob([data.body], { type: data.body.type });
            const a = document.createElement('a');
            a.setAttribute('style', 'display:none;');
            document.body.appendChild(a);
            a.download = fileName;
            a.href = URL.createObjectURL(downloadedFile);
            a.target = '_blank';
            a.click();
            document.body.removeChild(a);
            break;
    }
}