import { createFileRoute } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";
import ListIcon from "@/assets/icons/list.svg?react";
import { BgtCardList } from "@/components/BgtLayout/BgtCardList";
import { BgtEmptyPage } from "@/components/BgtLayout/BgtEmptyPage";
import { BgtPage } from "@/components/BgtLayout/BgtPage";
import { BgtPageContent } from "@/components/BgtLayout/BgtPageContent";
import BgtPageHeader from "@/components/BgtLayout/BgtPageHeader";
import { SuggestionStatus } from "@/models";
import { getSuggestions } from "@/services/queries/suggestions";
import { SuggestionCard } from "./-components/SuggestionCard";
import { useReviewQueue } from "./-hooks/useReviewQueue";

export const Route = createFileRoute("/review-queue/")({
	component: RouteComponent,
	loader: ({ context: { queryClient } }) => {
		queryClient.prefetchQuery(getSuggestions(SuggestionStatus.Pending));
	},
});

function RouteComponent() {
	const { t } = useTranslation(["review-queue", "common"]);
	const { suggestions, isLoading, approve, reject, retry } = useReviewQueue();

	if (isLoading) return null;

	if (suggestions.length === 0) {
		return (
			<BgtEmptyPage header={t("title")} icon={ListIcon} title={t("empty.title")} description={t("empty.description")} />
		);
	}

	return (
		<BgtPage>
			<BgtPageHeader header={t("title")} icon={ListIcon} />
			<BgtPageContent>
				<BgtCardList>
					{suggestions.map((suggestion) => (
						<SuggestionCard
							key={suggestion.id}
							suggestion={suggestion}
							onApprove={approve}
							onReject={(id, note) => reject({ id, note })}
							onRetry={(id, note) => retry({ id, note })}
						/>
					))}
				</BgtCardList>
			</BgtPageContent>
		</BgtPage>
	);
}
