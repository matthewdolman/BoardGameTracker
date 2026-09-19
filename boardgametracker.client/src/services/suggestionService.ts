import type { Suggestion, SuggestionStatus } from "../models";
import { axiosInstance } from "../utils/axiosInstance";

const domain = "suggestion";

export const getSuggestionsCall = (status?: SuggestionStatus): Promise<Suggestion[]> => {
	return axiosInstance.get<Suggestion[]>(domain, { params: { status } }).then((response) => {
		return response.data;
	});
};

export const approveSuggestionCall = (id: number): Promise<Suggestion> => {
	return axiosInstance.put<Suggestion>(`${domain}/${id}/approve`).then((response) => {
		return response.data;
	});
};

export const rejectSuggestionCall = (id: number, note?: string): Promise<Suggestion> => {
	return axiosInstance.put<Suggestion>(`${domain}/${id}/reject`, { note }).then((response) => {
		return response.data;
	});
};

export const retrySuggestionCall = (id: number, note?: string): Promise<Suggestion> => {
	return axiosInstance.put<Suggestion>(`${domain}/${id}/retry`, { note }).then((response) => {
		return response.data;
	});
};
