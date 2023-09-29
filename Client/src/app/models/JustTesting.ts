export interface JustTesting {
  id: string;
  scholarshipApplicantId: string;
  scholarshipId: string;
  scholarshipFormAnswer: ScholarshipFormAnswer[];
  dateCreated: string;
  dateModified: string;
  status: number;
}

export interface ScholarshipFormAnswer {
  name: string;
  value: string;
}
