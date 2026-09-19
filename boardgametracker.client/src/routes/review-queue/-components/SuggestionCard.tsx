import { useState } from "react";
import { useTranslation } from "react-i18next";
import LinkIcon from "@/assets/icons/arrow-square-out.svg?react";
import { BgtAvatar } from "@/components/BgtAvatar/BgtAvatar";
import BgtButton from "@/components/BgtButton/BgtButton";
import { BgtSimpleInputField } from "@/components/BgtForm";
import { BgtText } from "@/components/BgtText/BgtText";
import type { Suggestion } from "@/models";

interface Props {
	suggestion: Suggestion;
	onApprove: (id: number) => void;
	onReject: (id: number, note?: string) => void;
	onRetry: (id: number, note?: string) => void;
}

export const SuggestionCard = ({ suggestion, onApprove, onReject, onRetry }: Props) => {
	const { t } = useTranslation(["review-queue"]);
	const [noteMode, setNoteMode] = useState<"reject" | "retry" | null>(null);
	const [note, setNote] = useState("");

	const image = suggestion.bggThumbnailUrl ?? suggestion.photoImagePath;
	const isMatched = suggestion.bggId != null;

	const submitNote = () => {
		if (noteMode === "reject") onReject(suggestion.id, note || undefined);
		if (noteMode === "retry") onRetry(suggestion.id, note || undefined);
		setNoteMode(null);
		setNote("");
	};

	return (
		<div className="bg-primary/10 border border-primary/20 rounded-lg p-4 flex flex-col gap-3">
			<div className="flex flex-row gap-3 items-start">
				<BgtAvatar title={suggestion.suggestedName} image={image} size="large" />
				<div className="flex flex-col gap-1 flex-1 min-w-0">
					<BgtText weight="medium" className="truncate">
						{suggestion.suggestedName}
					</BgtText>
					{isMatched ? (
						<a
							className="underline text-blue-700 text-sm flex flex-row items-center gap-1 w-fit"
							href={`https://boardgamegeek.com/boardgame/${suggestion.bggId}`}
							target="_blank"
							rel="noopener noreferrer"
						>
							{t("matched")} <LinkIcon className="size-3" />
						</a>
					) : (
						<BgtText size="2" color="white" opacity={50}>
							{t("unmatched")}
						</BgtText>
					)}
				</div>
			</div>

			{noteMode ? (
				<div className="flex flex-col gap-2">
					<BgtSimpleInputField
						type="text"
						value={note}
						placeholder={t(noteMode === "retry" ? "notes.retry-placeholder" : "notes.reject-placeholder")}
						onChange={(event) => setNote(event.target.value)}
					/>
					<div className="flex flex-row gap-2">
						<BgtButton size="1" variant="primary" onClick={submitNote}>
							{t("confirm")}
						</BgtButton>
						<BgtButton size="1" variant="cancel" onClick={() => setNoteMode(null)}>
							{t("cancel")}
						</BgtButton>
					</div>
				</div>
			) : (
				<div className="flex flex-row gap-2">
					<BgtButton size="1" variant="primary" onClick={() => onApprove(suggestion.id)}>
						{t("approve")}
					</BgtButton>
					<BgtButton size="1" variant="cancel" onClick={() => setNoteMode("retry")}>
						{t("retry")}
					</BgtButton>
					<BgtButton size="1" variant="error" onClick={() => setNoteMode("reject")}>
						{t("reject")}
					</BgtButton>
				</div>
			)}
		</div>
	);
};
