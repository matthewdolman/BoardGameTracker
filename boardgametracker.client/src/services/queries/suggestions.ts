import { queryOptions } from "@tanstack/react-query";
import { QUERY_KEYS, type SuggestionStatus } from "@/models";
import { getSuggestionsCall } from "../suggestionService";

export const getSuggestions = (status?: SuggestionStatus) =>
	queryOptions({
		queryKey: [QUERY_KEYS.suggestions, status ?? "all"],
		queryFn: () => getSuggestionsCall(status),
	});
