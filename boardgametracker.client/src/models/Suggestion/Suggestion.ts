export enum SuggestionStatus {
	Pending = "pending",
	Approved = "approved",
	Rejected = "rejected",
	RetryRequested = "retryRequested",
}

export interface Suggestion {
	id: number;
	status: SuggestionStatus;
	suggestedName: string;
	bggId: number | null;
	bggThumbnailUrl: string | null;
	photoImagePath: string;
	sourceNote: string | null;
	reviewNote: string | null;
	createdAt: string;
	reviewedAt: string | null;
}
