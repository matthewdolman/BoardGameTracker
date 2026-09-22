import { useMutation, useQuery } from "@tanstack/react-query";
import { useQueryInvalidator } from "@/hooks/useQueryInvalidator";
import { useToasts } from "@/routes/-hooks/useToasts";
import { createApiTokenCall, revokeApiTokenCall } from "@/services/apiTokenService";
import { getApiTokens } from "@/services/queries/apiTokens";

export const useApiTokenData = () => {
	const invalidator = useQueryInvalidator();
	const { successToast, errorToast } = useToasts();

	const tokensQuery = useQuery(getApiTokens());

	const createTokenMutation = useMutation({
		mutationFn: createApiTokenCall,
		onSuccess: async () => {
			await invalidator.invalidateApiTokens();
			successToast("settings:api-tokens.notifications.created");
		},
		onError: () => {
			errorToast("settings:api-tokens.notifications.create-failed");
		},
	});

	const revokeTokenMutation = useMutation({
		mutationFn: revokeApiTokenCall,
		onSuccess: async () => {
			await invalidator.invalidateApiTokens();
			successToast("settings:api-tokens.notifications.revoked");
		},
		onError: () => {
			errorToast("settings:api-tokens.notifications.revoke-failed");
		},
	});

	return {
		tokens: tokensQuery.data ?? [],
		isLoading: tokensQuery.isLoading,
		createToken: createTokenMutation.mutateAsync,
		isCreating: createTokenMutation.isPending,
		revokeToken: revokeTokenMutation.mutateAsync,
	};
};
