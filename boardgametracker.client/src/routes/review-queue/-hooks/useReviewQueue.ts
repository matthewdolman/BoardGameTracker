import { useMutation, useQuery } from "@tanstack/react-query";
import { useQueryInvalidator } from "@/hooks/useQueryInvalidator";
import { SuggestionStatus } from "@/models";
import { useToasts } from "@/routes/-hooks/useToasts";
import { getSuggestions } from "@/services/queries/suggestions";
import { approveSuggestionCall, rejectSuggestionCall, retrySuggestionCall } from "@/services/suggestionService";

export const useReviewQueue = () => {
	const invalidator = useQueryInvalidator();
	const { successToast, errorToast } = useToasts();

	const { data, isLoading } = useQuery(getSuggestions(SuggestionStatus.Pending));

	const approveMutation = useMutation({
		mutationFn: approveSuggestionCall,
		async onSuccess() {
			await Promise.all([
				invalidator.invalidateSuggestions(),
				invalidator.invalidateGames(),
				invalidator.invalidateCounts(),
				invalidator.invalidateDashboard(),
			]);
			successToast("review-queue:notifications.approved");
		},
		onError() {
			errorToast("review-queue:notifications.approve-failed");
		},
	});

	const rejectMutation = useMutation({
		mutationFn: ({ id, note }: { id: number; note?: string }) => rejectSuggestionCall(id, note),
		async onSuccess() {
			await invalidator.invalidateSuggestions();
			successToast("review-queue:notifications.rejected");
		},
		onError() {
			errorToast("review-queue:notifications.reject-failed");
		},
	});

	const retryMutation = useMutation({
		mutationFn: ({ id, note }: { id: number; note?: string }) => retrySuggestionCall(id, note),
		async onSuccess() {
			await invalidator.invalidateSuggestions();
			successToast("review-queue:notifications.retry-requested");
		},
		onError() {
			errorToast("review-queue:notifications.retry-failed");
		},
	});

	return {
		suggestions: data ?? [],
		isLoading,
		approve: approveMutation.mutate,
		reject: rejectMutation.mutate,
		retry: retryMutation.mutate,
	};
};
