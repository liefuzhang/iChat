--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: ChannelSubscriptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChannelSubscriptions" (
    "ChannelId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "TRIAL639" character(1)
);


ALTER TABLE public."ChannelSubscriptions" OWNER TO postgres;

--
-- Name: TABLE "ChannelSubscriptions"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."ChannelSubscriptions" IS 'TRIAL';


--
-- Name: COLUMN "ChannelSubscriptions"."ChannelId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ChannelSubscriptions"."ChannelId" IS 'TRIAL';


--
-- Name: COLUMN "ChannelSubscriptions"."UserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ChannelSubscriptions"."UserId" IS 'TRIAL';


--
-- Name: COLUMN "ChannelSubscriptions"."TRIAL639"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ChannelSubscriptions"."TRIAL639" IS 'TRIAL';


--
-- Name: Channels; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Channels" (
    "Id" integer NOT NULL,
    "Name" text,
    "Topic" text,
    "WorkspaceId" integer DEFAULT 0 NOT NULL,
    "CreatedByUserId" integer DEFAULT 1 NOT NULL,
    "CreatedDate" timestamp(6) without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "TRIAL636" character(1)
);


ALTER TABLE public."Channels" OWNER TO postgres;

--
-- Name: TABLE "Channels"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Channels" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."Name"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."Name" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."Topic"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."Topic" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."CreatedByUserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."CreatedByUserId" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "Channels"."TRIAL636"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Channels"."TRIAL636" IS 'TRIAL';


--
-- Name: Channels_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Channels_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Channels_Id_seq" OWNER TO postgres;

--
-- Name: Channels_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Channels_Id_seq" OWNED BY public."Channels"."Id";


--
-- Name: ConversationUsers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ConversationUsers" (
    "ConversationId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "TRIAL643" character(1)
);


ALTER TABLE public."ConversationUsers" OWNER TO postgres;

--
-- Name: TABLE "ConversationUsers"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."ConversationUsers" IS 'TRIAL';


--
-- Name: COLUMN "ConversationUsers"."ConversationId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ConversationUsers"."ConversationId" IS 'TRIAL';


--
-- Name: COLUMN "ConversationUsers"."UserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ConversationUsers"."UserId" IS 'TRIAL';


--
-- Name: COLUMN "ConversationUsers"."TRIAL643"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ConversationUsers"."TRIAL643" IS 'TRIAL';


--
-- Name: Conversations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Conversations" (
    "Id" integer NOT NULL,
    "WorkspaceId" integer NOT NULL,
    "CreatedByUserId" integer DEFAULT 1 NOT NULL,
    "CreatedDate" timestamp(6) without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "IsPrivate" boolean DEFAULT false NOT NULL,
    "IsSelfConversation" boolean DEFAULT false NOT NULL,
    "TRIAL643" character(1)
);


ALTER TABLE public."Conversations" OWNER TO postgres;

--
-- Name: TABLE "Conversations"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Conversations" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."CreatedByUserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."CreatedByUserId" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."IsPrivate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."IsPrivate" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."IsSelfConversation"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."IsSelfConversation" IS 'TRIAL';


--
-- Name: COLUMN "Conversations"."TRIAL643"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Conversations"."TRIAL643" IS 'TRIAL';


--
-- Name: Conversations_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Conversations_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Conversations_Id_seq" OWNER TO postgres;

--
-- Name: Conversations_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Conversations_Id_seq" OWNED BY public."Conversations"."Id";


--
-- Name: Files; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Files" (
    "Id" integer NOT NULL,
    "Name" text,
    "UploadedByUserId" integer NOT NULL,
    "UploadedDate" timestamp(6) without time zone NOT NULL,
    "WorkspaceId" integer NOT NULL,
    "SavedName" text,
    "ContentType" text,
    "TRIAL646" character(1)
);


ALTER TABLE public."Files" OWNER TO postgres;

--
-- Name: TABLE "Files"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Files" IS 'TRIAL';


--
-- Name: COLUMN "Files"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Files"."Name"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."Name" IS 'TRIAL';


--
-- Name: COLUMN "Files"."UploadedByUserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."UploadedByUserId" IS 'TRIAL';


--
-- Name: COLUMN "Files"."UploadedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."UploadedDate" IS 'TRIAL';


--
-- Name: COLUMN "Files"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "Files"."SavedName"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."SavedName" IS 'TRIAL';


--
-- Name: COLUMN "Files"."ContentType"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."ContentType" IS 'TRIAL';


--
-- Name: COLUMN "Files"."TRIAL646"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Files"."TRIAL646" IS 'TRIAL';


--
-- Name: Files_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Files_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Files_Id_seq" OWNER TO postgres;

--
-- Name: Files_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Files_Id_seq" OWNED BY public."Files"."Id";


--
-- Name: MessageFileAttachments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MessageFileAttachments" (
    "MessageId" integer NOT NULL,
    "FileId" integer NOT NULL,
    "TRIAL649" character(1)
);


ALTER TABLE public."MessageFileAttachments" OWNER TO postgres;

--
-- Name: TABLE "MessageFileAttachments"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."MessageFileAttachments" IS 'TRIAL';


--
-- Name: COLUMN "MessageFileAttachments"."MessageId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageFileAttachments"."MessageId" IS 'TRIAL';


--
-- Name: COLUMN "MessageFileAttachments"."FileId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageFileAttachments"."FileId" IS 'TRIAL';


--
-- Name: COLUMN "MessageFileAttachments"."TRIAL649"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageFileAttachments"."TRIAL649" IS 'TRIAL';


--
-- Name: MessageReactionUsers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MessageReactionUsers" (
    "MessageReactionId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "TRIAL656" character(1)
);


ALTER TABLE public."MessageReactionUsers" OWNER TO postgres;

--
-- Name: TABLE "MessageReactionUsers"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."MessageReactionUsers" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactionUsers"."MessageReactionId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactionUsers"."MessageReactionId" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactionUsers"."UserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactionUsers"."UserId" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactionUsers"."TRIAL656"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactionUsers"."TRIAL656" IS 'TRIAL';


--
-- Name: MessageReactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MessageReactions" (
    "Id" integer NOT NULL,
    "MessageId" integer NOT NULL,
    "EmojiColons" text,
    "CreatedDate" timestamp(6) without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "TRIAL653" character(1)
);


ALTER TABLE public."MessageReactions" OWNER TO postgres;

--
-- Name: TABLE "MessageReactions"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."MessageReactions" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactions"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactions"."Id" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactions"."MessageId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactions"."MessageId" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactions"."EmojiColons"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactions"."EmojiColons" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactions"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactions"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "MessageReactions"."TRIAL653"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."MessageReactions"."TRIAL653" IS 'TRIAL';


--
-- Name: MessageReactions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MessageReactions_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MessageReactions_Id_seq" OWNER TO postgres;

--
-- Name: MessageReactions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MessageReactions_Id_seq" OWNED BY public."MessageReactions"."Id";


--
-- Name: Messages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Messages" (
    "Id" integer NOT NULL,
    "SenderId" integer NOT NULL,
    "Content" text,
    "CreatedDate" timestamp(6) without time zone NOT NULL,
    "Discriminator" text NOT NULL,
    "ChannelId" integer,
    "ConversationId" integer,
    "WorkspaceId" integer DEFAULT 0 NOT NULL,
    "HasFileAttachments" boolean DEFAULT false NOT NULL,
    "ContentEdited" boolean DEFAULT false NOT NULL,
    "IsSystemMessage" boolean DEFAULT false NOT NULL,
    "TRIAL659" character(1)
);


ALTER TABLE public."Messages" OWNER TO postgres;

--
-- Name: TABLE "Messages"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Messages" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."SenderId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."SenderId" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."Content"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."Content" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."Discriminator"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."Discriminator" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."ChannelId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."ChannelId" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."ConversationId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."ConversationId" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."HasFileAttachments"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."HasFileAttachments" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."ContentEdited"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."ContentEdited" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."IsSystemMessage"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."IsSystemMessage" IS 'TRIAL';


--
-- Name: COLUMN "Messages"."TRIAL659"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Messages"."TRIAL659" IS 'TRIAL';


--
-- Name: Messages_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Messages_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Messages_Id_seq" OWNER TO postgres;

--
-- Name: Messages_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Messages_Id_seq" OWNED BY public."Messages"."Id";


--
-- Name: ResetPasswordRequsets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ResetPasswordRequsets" (
    "Id" integer NOT NULL,
    "Email" text,
    "Resetted" boolean NOT NULL,
    "Cancelled" boolean NOT NULL,
    "ResetCode" uuid NOT NULL,
    "TRIAL662" character(1)
);


ALTER TABLE public."ResetPasswordRequsets" OWNER TO postgres;

--
-- Name: TABLE "ResetPasswordRequsets"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."ResetPasswordRequsets" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."Id" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."Email"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."Email" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."Resetted"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."Resetted" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."Cancelled"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."Cancelled" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."ResetCode"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."ResetCode" IS 'TRIAL';


--
-- Name: COLUMN "ResetPasswordRequsets"."TRIAL662"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."ResetPasswordRequsets"."TRIAL662" IS 'TRIAL';


--
-- Name: ResetPasswordRequsets_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ResetPasswordRequsets_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ResetPasswordRequsets_Id_seq" OWNER TO postgres;

--
-- Name: ResetPasswordRequsets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ResetPasswordRequsets_Id_seq" OWNED BY public."ResetPasswordRequsets"."Id";


--
-- Name: UserInvitations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserInvitations" (
    "Id" integer NOT NULL,
    "UserEmail" text,
    "InvitedByUserId" integer NOT NULL,
    "WorkspaceId" integer NOT NULL,
    "InviteDate" timestamp(6) without time zone NOT NULL,
    "Acceptted" boolean NOT NULL,
    "InvitationCode" uuid NOT NULL,
    "Cancelled" boolean DEFAULT false NOT NULL,
    "TRIAL666" character(1)
);


ALTER TABLE public."UserInvitations" OWNER TO postgres;

--
-- Name: TABLE "UserInvitations"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."UserInvitations" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."Id" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."UserEmail"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."UserEmail" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."InvitedByUserId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."InvitedByUserId" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."InviteDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."InviteDate" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."Acceptted"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."Acceptted" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."InvitationCode"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."InvitationCode" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."Cancelled"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."Cancelled" IS 'TRIAL';


--
-- Name: COLUMN "UserInvitations"."TRIAL666"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."UserInvitations"."TRIAL666" IS 'TRIAL';


--
-- Name: UserInvitations_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserInvitations_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."UserInvitations_Id_seq" OWNER TO postgres;

--
-- Name: UserInvitations_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserInvitations_Id_seq" OWNED BY public."UserInvitations"."Id";


--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Email" text,
    "Status" integer NOT NULL,
    "DisplayName" text,
    "PasswordHash" bytea,
    "PasswordSalt" bytea,
    "WorkspaceId" integer DEFAULT 0 NOT NULL,
    "CreatedDate" timestamp(6) without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "IdenticonGuid" uuid NOT NULL,
    "PhoneNumber" text,
    "TRIAL669" character(1)
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: TABLE "Users"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Users" IS 'TRIAL';


--
-- Name: COLUMN "Users"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Users"."Email"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."Email" IS 'TRIAL';


--
-- Name: COLUMN "Users"."Status"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."Status" IS 'TRIAL';


--
-- Name: COLUMN "Users"."DisplayName"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."DisplayName" IS 'TRIAL';


--
-- Name: COLUMN "Users"."PasswordHash"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."PasswordHash" IS 'TRIAL';


--
-- Name: COLUMN "Users"."PasswordSalt"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."PasswordSalt" IS 'TRIAL';


--
-- Name: COLUMN "Users"."WorkspaceId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."WorkspaceId" IS 'TRIAL';


--
-- Name: COLUMN "Users"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "Users"."IdenticonGuid"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."IdenticonGuid" IS 'TRIAL';


--
-- Name: COLUMN "Users"."PhoneNumber"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."PhoneNumber" IS 'TRIAL';


--
-- Name: COLUMN "Users"."TRIAL669"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Users"."TRIAL669" IS 'TRIAL';


--
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Users_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Users_Id_seq" OWNER TO postgres;

--
-- Name: Users_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Users_Id_seq" OWNED BY public."Users"."Id";


--
-- Name: Workspaces; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Workspaces" (
    "Id" integer NOT NULL,
    "Name" text,
    "OwnerId" integer NOT NULL,
    "CreatedDate" timestamp(6) without time zone NOT NULL,
    "TRIAL672" character(1)
);


ALTER TABLE public."Workspaces" OWNER TO postgres;

--
-- Name: TABLE "Workspaces"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."Workspaces" IS 'TRIAL';


--
-- Name: COLUMN "Workspaces"."Id"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Workspaces"."Id" IS 'TRIAL';


--
-- Name: COLUMN "Workspaces"."Name"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Workspaces"."Name" IS 'TRIAL';


--
-- Name: COLUMN "Workspaces"."OwnerId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Workspaces"."OwnerId" IS 'TRIAL';


--
-- Name: COLUMN "Workspaces"."CreatedDate"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Workspaces"."CreatedDate" IS 'TRIAL';


--
-- Name: COLUMN "Workspaces"."TRIAL672"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."Workspaces"."TRIAL672" IS 'TRIAL';


--
-- Name: Workspaces_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Workspaces_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Workspaces_Id_seq" OWNER TO postgres;

--
-- Name: Workspaces_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Workspaces_Id_seq" OWNED BY public."Workspaces"."Id";


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    "TRIAL633" character(1)
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Name: TABLE "__EFMigrationsHistory"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."__EFMigrationsHistory" IS 'TRIAL';


--
-- Name: COLUMN "__EFMigrationsHistory"."MigrationId"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."__EFMigrationsHistory"."MigrationId" IS 'TRIAL';


--
-- Name: COLUMN "__EFMigrationsHistory"."ProductVersion"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."__EFMigrationsHistory"."ProductVersion" IS 'TRIAL';


--
-- Name: COLUMN "__EFMigrationsHistory"."TRIAL633"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."__EFMigrationsHistory"."TRIAL633" IS 'TRIAL';


--
-- Name: Channels Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Channels" ALTER COLUMN "Id" SET DEFAULT nextval('public."Channels_Id_seq"'::regclass);


--
-- Name: Conversations Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Conversations" ALTER COLUMN "Id" SET DEFAULT nextval('public."Conversations_Id_seq"'::regclass);


--
-- Name: Files Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Files" ALTER COLUMN "Id" SET DEFAULT nextval('public."Files_Id_seq"'::regclass);


--
-- Name: MessageReactions Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactions" ALTER COLUMN "Id" SET DEFAULT nextval('public."MessageReactions_Id_seq"'::regclass);


--
-- Name: Messages Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages" ALTER COLUMN "Id" SET DEFAULT nextval('public."Messages_Id_seq"'::regclass);


--
-- Name: ResetPasswordRequsets Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ResetPasswordRequsets" ALTER COLUMN "Id" SET DEFAULT nextval('public."ResetPasswordRequsets_Id_seq"'::regclass);


--
-- Name: UserInvitations Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserInvitations" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserInvitations_Id_seq"'::regclass);


--
-- Name: Users Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "Id" SET DEFAULT nextval('public."Users_Id_seq"'::regclass);


--
-- Name: Workspaces Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Workspaces" ALTER COLUMN "Id" SET DEFAULT nextval('public."Workspaces_Id_seq"'::regclass);


--
-- Data for Name: ChannelSubscriptions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ChannelSubscriptions" ("ChannelId", "UserId", "TRIAL639") FROM stdin;
5	3	T
6	3	T
7	3	T
9	3	T
10	3	T
12	4	T
13	4	T
14	4	T
15	5	T
16	5	T
25	5	T
26	5	T
27	5	T
28	5	T
17	6	T
18	6	T
19	7	T
20	7	T
21	8	T
22	8	T
21	9	T
22	9	T
21	10	T
22	10	T
15	11	T
16	11	T
23	11	T
24	11	T
26	11	T
15	12	T
16	12	T
26	12	T
28	12	T
15	13	T
16	13	T
24	13	T
26	13	T
27	13	T
28	13	T
15	14	T
16	14	T
23	14	T
26	14	T
15	15	T
16	15	T
24	15	T
26	15	T
27	15	T
28	15	T
15	16	T
16	16	T
15	17	T
16	17	T
5	18	T
6	18	T
29	19	T
30	19	T
17	20	T
18	20	T
17	21	T
18	21	T
\.


--
-- Data for Name: Channels; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Channels" ("Id", "Name", "Topic", "WorkspaceId", "CreatedByUserId", "CreatedDate", "TRIAL636") FROM stdin;
5	general	Anything that's talkable	5	3	2019-08-04 11:40:50.751916	T
6	random	Another random channel	5	3	2019-08-04 11:40:50.789013	T
7	meetings	meeting arrangements	5	3	2019-08-05 00:18:40.360329	T
8	meetings	meeting arrangements	5	3	2019-08-05 00:18:40.360875	T
9	meetings1	meeting arrangements11	5	3	2019-08-05 00:19:10.276554	T
10	another		5	3	2019-08-05 00:24:50.650595	T
11	hello		5	3	2019-08-05 00:25:00.978271	T
12	general	Anything that's talkable	7	4	2019-08-06 07:42:21.992492	T
13	random	Another random channel	7	4	2019-08-06 07:42:22.016944	T
14	meetings	123	7	4	2019-08-06 07:42:40.9686	T
15	general	Anything that's talkable	8	5	2019-08-06 23:18:05.415121	T
16	random	Another random channel	8	5	2019-08-06 23:18:05.440623	T
17	general	Anything that's talkable	9	6	2019-08-12 09:16:02.734984	T
18	random	Another random channel	9	6	2019-08-12 09:16:02.924453	T
19	general	Anything that's talkable	10	7	2019-08-12 09:20:22.552308	T
20	random	Another random channel	10	7	2019-08-12 09:20:22.558296	T
21	general	Anything that's talkable	11	8	2019-08-12 09:41:56.183012	T
22	random	Another random channel	11	8	2019-08-12 09:41:56.19673	T
23	news	Any recent news that you want to share	8	11	2019-08-29 09:25:01.696939	T
24	fun_stuff		8	11	2019-08-29 09:25:36.138701	T
25	weather		8	5	2019-09-02 02:31:00.381763	T
26	carpool		8	5	2019-09-02 02:31:13.742789	T
27	activities		8	5	2019-09-02 02:31:25.980462	T
28	sports		8	5	2019-09-02 02:31:33.273272	T
29	general	Anything that's talkable	13	19	2020-10-10 02:07:12.989993	T
30	random	Another random channel	13	19	2020-10-10 02:07:13.019179	T
\.


--
-- Data for Name: ConversationUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ConversationUsers" ("ConversationId", "UserId", "TRIAL643") FROM stdin;
3	3	T
4	4	T
5	5	T
22	5	T
27	5	T
28	5	T
29	5	T
6	6	T
35	6	T
38	6	T
7	7	T
8	8	T
10	8	T
9	9	T
10	9	T
11	10	T
12	11	T
14	11	T
20	11	T
22	11	T
25	11	T
26	11	T
27	11	T
28	11	T
13	12	T
14	12	T
20	12	T
21	12	T
25	12	T
26	12	T
27	12	T
15	13	T
17	13	T
19	13	T
22	13	T
24	13	T
25	13	T
26	13	T
28	13	T
16	14	T
17	14	T
22	14	T
23	14	T
24	14	T
26	14	T
18	15	T
19	15	T
20	15	T
21	15	T
22	15	T
23	15	T
24	15	T
25	15	T
26	15	T
27	15	T
28	15	T
29	15	T
31	16	T
30	17	T
32	18	T
33	19	T
34	20	T
35	20	T
37	20	T
38	20	T
36	21	T
37	21	T
38	21	T
\.


--
-- Data for Name: Conversations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Conversations" ("Id", "WorkspaceId", "CreatedByUserId", "CreatedDate", "IsPrivate", "IsSelfConversation", "TRIAL643") FROM stdin;
3	5	3	2019-08-04 11:40:50.997115	f	t	T
4	7	4	2019-08-06 07:42:22.122728	f	t	T
5	8	5	2019-08-06 23:18:05.613597	f	t	T
6	9	6	2019-08-12 09:16:03.100149	f	t	T
7	10	7	2019-08-12 09:20:22.594371	f	t	T
8	11	8	2019-08-12 09:41:56.267952	f	t	T
9	11	9	2019-08-12 09:45:53.450803	f	t	T
10	11	9	2019-08-12 09:46:06.870605	t	f	T
11	11	10	2019-08-12 09:50:53.402936	f	t	T
12	8	11	2019-08-27 04:06:45.964926	f	t	T
13	8	12	2019-08-27 04:07:22.887863	f	t	T
14	8	11	2019-08-29 09:38:06.403268	t	f	T
15	8	13	2019-09-02 02:37:35.2398	f	t	T
16	8	14	2019-09-02 02:48:05.68396	f	t	T
17	8	14	2019-09-02 02:51:25.002549	t	f	T
18	8	15	2019-09-02 04:11:44.96454	f	t	T
19	8	13	2019-09-02 04:12:47.369496	t	f	T
20	8	15	2019-09-02 04:13:07.808081	f	f	T
21	8	15	2019-09-02 04:21:04.159717	t	f	T
22	8	15	2019-09-02 04:21:19.80857	f	f	T
23	8	15	2019-09-03 10:17:48.628306	t	f	T
24	8	15	2019-09-03 10:18:23.781874	f	f	T
25	8	15	2019-09-03 10:18:33.646629	f	f	T
26	8	15	2019-09-03 10:18:57.611677	f	f	T
27	8	15	2019-09-25 08:31:21.133151	f	f	T
28	8	15	2019-09-25 08:31:56.429176	f	f	T
29	8	15	2019-09-25 08:32:02.582462	t	f	T
30	8	17	2020-10-10 14:51:34.471955	f	t	T
31	8	16	2020-10-10 14:51:34.47195	f	t	T
32	5	18	2020-10-10 14:54:46.023392	f	t	T
33	13	19	2020-10-10 02:07:13.360494	f	t	T
34	9	20	2020-10-10 02:08:30.706275	f	t	T
35	9	6	2020-10-10 02:09:24.796397	t	f	T
36	9	21	2020-10-10 02:22:34.214288	f	t	T
37	9	21	2020-10-10 02:23:53.651999	t	f	T
38	9	21	2020-10-10 02:29:58.569992	f	f	T
\.


--
-- Data for Name: Files; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Files" ("Id", "Name", "UploadedByUserId", "UploadedDate", "WorkspaceId", "SavedName", "ContentType", "TRIAL646") FROM stdin;
1	keybindings.json	3	2019-08-05 00:20:43.213816	5	204a0655-158a-40df-bf12-df38b1b0b4f2.json	application/json	T
2	image.png	4	2019-08-19 10:01:17.292372	7	61d36cd1-5ab5-49da-9a5d-fbea15e296b2.png	image/png	T
3	windows-version.txt	11	2019-08-29 09:16:02.474498	8	c392c1e2-8f67-4930-980f-1dccdea30f3c.txt	text/plain	T
4	image.png	11	2019-08-29 09:17:15.012332	8	add00c5b-71cd-4002-af4e-e908596f72ab.png	image/png	T
5	favicon.ico	15	2019-09-04 10:36:36.667206	8	434a1a6f-c070-4126-9a8c-b6cf4c2a32e0.ico	image/x-icon	T
6	image.png	21	2020-10-10 02:24:35.898546	9	22958b2a-90d8-4689-b3d3-c5e069b6605d.png	image/png	T
7	image.png	20	2020-10-10 02:26:47.606781	9	fc66bf5d-3e1e-4d17-9c2d-f6de9d7fc799.png	image/png	T
8	Screenshot (1).png	21	2020-10-10 02:30:38.828965	9	56bfa4b4-b9a1-4c6e-b9d2-16dd29331e6b.png	image/png	T
9	IMG_9089.JPG	20	2020-11-10 07:57:27.306152	9	9fc118a8-b24c-4043-8570-1c932f16c54b.JPG	image/jpeg	T
\.


--
-- Data for Name: MessageFileAttachments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."MessageFileAttachments" ("MessageId", "FileId", "TRIAL649") FROM stdin;
25	1	T
26	1	T
79	2	T
106	3	T
107	4	T
195	5	T
219	6	T
222	7	T
228	8	T
232	9	T
\.


--
-- Data for Name: MessageReactionUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."MessageReactionUsers" ("MessageReactionId", "UserId", "TRIAL656") FROM stdin;
1	3	T
4	4	T
6	4	T
7	11	T
8	12	T
9	12	T
10	12	T
11	12	T
12	15	T
14	20	T
15	20	T
16	20	T
13	21	T
14	21	T
15	21	T
\.


--
-- Data for Name: MessageReactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."MessageReactions" ("Id", "MessageId", "EmojiColons", "CreatedDate", "TRIAL653") FROM stdin;
1	12	:grinning:	2019-08-04 11:41:32.033604	T
4	59	:smiley:	2019-08-06 08:05:29.177385	T
6	59	:blush:	2019-08-19 10:00:30.339869	T
7	84	:+1:	2019-08-29 09:09:33.138639	T
8	104	:ok:	2019-08-29 09:14:41.090136	T
9	112	:+1:	2019-08-29 09:23:05.675745	T
10	112	:grinning:	2019-08-29 09:23:15.219476	T
11	130	:smiley:	2019-08-29 10:11:07.473029	T
12	104	:+1:	2019-09-03 10:24:46.532191	T
13	218	:zipper_mouth_face:	2020-10-10 02:25:03.892547	T
14	223	:sleepy:	2020-10-10 02:27:10.934403	T
15	223	:triumph:	2020-10-10 02:27:18.545064	T
16	225	:face_with_monocle:	2020-10-10 02:28:23.187705	T
\.


--
-- Data for Name: Messages; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Messages" ("Id", "SenderId", "Content", "CreatedDate", "Discriminator", "ChannelId", "ConversationId", "WorkspaceId", "HasFileAttachments", "ContentEdited", "IsSystemMessage", "TRIAL659") FROM stdin;
10	3	joined # general.	2019-08-04 11:40:50.867776	ChannelMessage	5	\N	5	f	f	t	T
11	3	joined # random.	2019-08-04 11:40:50.897224	ChannelMessage	6	\N	5	f	f	t	T
12	3	<p>1233123</p>	2019-08-04 11:40:57.408057	ChannelMessage	5	\N	5	f	f	f	T
13	3	<p>123123</p>	2019-08-04 11:40:59.954253	ChannelMessage	5	\N	5	f	f	f	T
14	3	<p><b>dfs</b></p>	2019-08-04 11:41:57.234442	ConversationMessage	\N	3	5	f	f	f	T
15	3	<p>lol</p>	2019-08-04 21:33:12.763861	ConversationMessage	\N	3	5	f	f	f	T
16	3	<p>123</p>	2019-08-04 21:33:25.044454	ConversationMessage	\N	3	5	f	f	f	T
17	3	<p>123</p>	2019-08-04 21:33:36.333736	ChannelMessage	6	\N	5	f	f	f	T
18	3	<p>123</p>	2019-08-04 21:33:39.682605	ChannelMessage	5	\N	5	f	f	f	T
19	3	joined # meetings.	2019-08-05 00:18:40.527714	ChannelMessage	7	\N	5	f	f	t	T
20	3	joined # meetings.	2019-08-05 00:18:40.545456	ChannelMessage	8	\N	5	f	f	t	T
21	3	joined # meetings1.	2019-08-05 00:19:10.289947	ChannelMessage	9	\N	5	f	f	t	T
22	3	<p>123</p>	2019-08-05 00:19:32.179564	ChannelMessage	7	\N	5	f	f	f	T
24	3	<p>some type</p>	2019-08-05 00:19:50.76398	ChannelMessage	7	\N	5	f	t	f	T
25	3		2019-08-05 00:20:43.239783	ChannelMessage	7	\N	5	t	f	f	T
26	3		2019-08-05 00:21:09.644617	ConversationMessage	\N	3	5	t	f	f	T
27	3	<p>cool</p>	2019-08-05 00:21:27.307489	ChannelMessage	6	\N	5	f	f	f	T
28	3	<p>hello</p>	2019-08-05 00:21:38.654015	ChannelMessage	5	\N	5	f	f	f	T
29	3	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 54.89904988123515%;background-size:5362.5% 5362.5%" data-codepoints="1f604"></span></span></span></span>锘?/span> </p>	2019-08-05 00:24:33.690716	ChannelMessage	7	\N	5	f	f	f	T
30	3	joined # another.	2019-08-05 00:24:50.670596	ChannelMessage	10	\N	5	f	f	t	T
31	3	joined # hello.	2019-08-05 00:25:00.988081	ChannelMessage	11	\N	5	f	f	t	T
32	3	joined # meetings1.	2019-08-05 00:25:24.475936	ChannelMessage	9	\N	5	f	f	t	T
33	3	<p>hi</p>	2019-08-05 10:04:38.94915	ChannelMessage	5	\N	5	f	f	f	T
34	3	<p>222</p>	2019-08-05 10:11:15.090317	ChannelMessage	5	\N	5	f	f	f	T
35	3	<p>123</p>	2019-08-05 10:56:38.137545	ChannelMessage	6	\N	5	f	f	f	T
36	3	<p>123</p>	2019-08-05 10:58:27.50743	ChannelMessage	5	\N	5	f	f	f	T
37	3	<p>123</p>	2019-08-05 10:58:32.21328	ChannelMessage	10	\N	5	f	f	f	T
40	3	<p>123</p>	2019-08-05 11:03:13.128199	ChannelMessage	7	\N	5	f	f	f	T
41	3	<p>123</p>	2019-08-06 07:39:56.739472	ChannelMessage	5	\N	5	f	f	f	T
42	3	<p>asdf</p>	2019-08-06 07:40:07.839055	ChannelMessage	9	\N	5	f	f	f	T
43	3	<p>fasdf</p>	2019-08-06 07:40:12.744893	ChannelMessage	9	\N	5	f	f	f	T
44	4	joined # general.	2019-08-06 07:42:22.060265	ChannelMessage	12	\N	7	f	f	t	T
45	4	joined # random.	2019-08-06 07:42:22.076465	ChannelMessage	13	\N	7	f	f	t	T
46	4	joined # meetings.	2019-08-06 07:42:40.982585	ChannelMessage	14	\N	7	f	f	t	T
47	4	<p>hello</p>	2019-08-06 07:42:47.960667	ChannelMessage	14	\N	7	f	f	f	T
48	4	<p>how are you</p>	2019-08-06 07:42:51.79392	ChannelMessage	14	\N	7	f	f	f	T
51	4	<p>asdfjaskdflasdflkj;l</p>	2019-08-06 07:43:10.680237	ConversationMessage	\N	4	7	f	f	f	T
52	4	<p>askdf</p>	2019-08-06 07:43:12.568947	ConversationMessage	\N	4	7	f	f	f	T
54	4	<p>asdfasdf</p>	2019-08-06 07:43:16.61217	ConversationMessage	\N	4	7	f	f	f	T
56	4	<p>@asdf</p>	2019-08-06 07:43:24.549585	ConversationMessage	\N	4	7	f	f	f	T
57	4	<p>@123</p>	2019-08-06 07:43:34.240677	ConversationMessage	\N	4	7	f	f	f	T
58	4	<p>@asd</p>	2019-08-06 07:43:42.280609	ConversationMessage	\N	4	7	f	f	f	T
59	4	<p><a href="https://www.stuff.co.nz/travel/destinations/nz/114778829/how-new-zealands-land-mass-compares-to-europe" target="_blank">https://www.stuff.co.nz/travel/destinations/nz/114778829/how-new-zealands-land-mass-compares-to-europe</a></p>	2019-08-06 08:04:12.779897	ChannelMessage	12	\N	7	f	f	f	T
60	5	joined # general.	2019-08-06 23:18:05.492	ChannelMessage	15	\N	8	f	f	t	T
61	5	joined # random.	2019-08-06 23:18:05.569756	ChannelMessage	16	\N	8	f	f	t	T
62	6	joined # general.	2019-08-12 09:16:02.971079	ChannelMessage	17	\N	9	f	f	t	T
63	6	joined # random.	2019-08-12 09:16:03.051827	ChannelMessage	18	\N	9	f	f	t	T
64	7	joined # general.	2019-08-12 09:20:22.572659	ChannelMessage	19	\N	10	f	f	t	T
65	7	joined # random.	2019-08-12 09:20:22.586776	ChannelMessage	20	\N	10	f	f	t	T
66	8	joined # general.	2019-08-12 09:41:56.219498	ChannelMessage	21	\N	11	f	f	t	T
67	8	joined # random.	2019-08-12 09:41:56.248595	ChannelMessage	22	\N	11	f	f	t	T
68	9	joined # general.	2019-08-12 09:45:53.286336	ChannelMessage	21	\N	11	f	f	t	T
69	9	joined # random.	2019-08-12 09:45:53.380414	ChannelMessage	22	\N	11	f	f	t	T
70	9	<p>hello</p>	2019-08-12 09:46:11.141714	ConversationMessage	\N	10	11	f	f	f	T
71	10	joined # general.	2019-08-12 09:50:53.380553	ChannelMessage	21	\N	11	f	f	t	T
72	10	joined # random.	2019-08-12 09:50:53.39492	ChannelMessage	22	\N	11	f	f	t	T
73	10	<p>Hi guys!</p>	2019-08-12 09:51:17.587668	ChannelMessage	21	\N	11	f	f	f	T
74	8	<p>What's up!</p>	2019-08-12 09:51:48.253546	ChannelMessage	21	\N	11	f	f	f	T
75	8	<p>Haven't seen you for a while</p>	2019-08-12 09:52:22.208541	ChannelMessage	21	\N	11	f	f	f	T
76	8	<p>How's life been?</p>	2019-08-12 09:52:36.062382	ChannelMessage	21	\N	11	f	f	f	T
79	4		2019-08-19 10:01:17.322696	ChannelMessage	12	\N	7	t	f	f	T
80	11	joined & general.	2019-08-27 04:06:45.836413	ChannelMessage	15	\N	8	f	f	t	T
81	11	joined & random.	2019-08-27 04:06:45.904823	ChannelMessage	16	\N	8	f	f	t	T
82	12	joined & general.	2019-08-27 04:07:22.865492	ChannelMessage	15	\N	8	f	f	t	T
83	12	joined & random.	2019-08-27 04:07:22.879471	ChannelMessage	16	\N	8	f	f	t	T
84	5	<p>Welcome to ichat!</p>	2019-08-27 04:09:40.685084	ChannelMessage	15	\N	8	f	f	f	T
85	5	<p>Let's try out some cool features</p>	2019-08-27 04:11:01.03409	ChannelMessage	15	\N	8	f	f	f	T
86	12	<p>Sure</p>	2019-08-27 05:03:50.00144	ChannelMessage	15	\N	8	f	f	f	T
87	5	<p>So basically you can format your messages</p>	2019-08-27 05:05:15.875345	ChannelMessage	15	\N	8	f	f	f	T
88	5	<p><b>bold</b></p>	2019-08-27 05:05:17.366317	ChannelMessage	15	\N	8	f	f	f	T
89	5	<p><code>some test</code></p>	2019-08-27 05:05:26.567219	ChannelMessage	15	\N	8	f	t	f	T
90	5	<pre></p><p>some </p><p>paragraphs</p><p></pre>	2019-08-27 05:06:24.647087	ChannelMessage	15	\N	8	f	f	f	T
91	5	<p><i>italics text</i></p>	2019-08-27 05:06:42.995745	ChannelMessage	15	\N	8	f	t	f	T
92	5	<p><strike>strike text</strike></p>	2019-08-27 05:08:17.881327	ChannelMessage	15	\N	8	f	t	f	T
93	5	<blockquote>some quote</blockquote>	2019-08-27 05:08:23.417386	ChannelMessage	15	\N	8	f	f	f	T
94	3	<p>*dfdsf dfas (*</p>	2019-08-27 09:26:21.449274	ChannelMessage	9	\N	5	f	f	f	T
95	3	<p><b>dfdsf </b>dfdf*</p>	2019-08-27 09:55:53.990238	ChannelMessage	5	\N	5	f	f	f	T
96	3	<p>asdfkdsdfsdf sadf  sdf*</p>	2019-08-27 09:56:00.949012	ChannelMessage	5	\N	5	f	f	f	T
97	3	<p>``lkfajsf</p>	2019-08-27 09:56:07.76958	ChannelMessage	5	\N	5	f	f	f	T
98	3	<p><code>askfd  dfka dfkas dfksad f</code></p>	2019-08-27 09:56:20.314666	ChannelMessage	5	\N	5	f	f	f	T
99	11	<p>That looks good</p>	2019-08-29 09:09:11.772377	ChannelMessage	15	\N	8	f	f	f	T
100	11	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 47.06057007125891%;background-size:5362.5% 5362.5%" data-codepoints="1f600"></span></span></span></span>锘?/span> </p>	2019-08-29 09:09:22.301649	ChannelMessage	15	\N	8	f	f	f	T
101	11	<p>We can also mention someone in a message like this <span class="span-tag">锘?span contenteditable="false"><span data-user-id="12" class="mentioned-user">@User C</span></span>锘?/span>     </p>	2019-08-29 09:10:39.570459	ChannelMessage	15	\N	8	f	f	f	T
102	11	<p>then the user would get notified</p>	2019-08-29 09:10:56.984133	ChannelMessage	15	\N	8	f	f	f	T
103	12	<p>Yep, I saw that!</p>	2019-08-29 09:12:58.995683	ChannelMessage	15	\N	8	f	f	f	T
104	11	<p>By hovering over a message, you could add a reaction to it. If the message was posted by yourself, you could also edit/delete it.</p>	2019-08-29 09:14:15.38535	ChannelMessage	15	\N	8	f	f	f	T
105	11	<p>You can also post a file / an image </p>	2019-08-29 09:15:28.830249	ChannelMessage	15	\N	8	f	f	f	T
106	11		2019-08-29 09:16:02.507111	ChannelMessage	15	\N	8	t	f	f	T
107	11		2019-08-29 09:17:15.020593	ChannelMessage	15	\N	8	t	f	f	T
108	11	<p>By hovering over a file, you have options to share it with other channels / conversations, or download the file</p>	2019-08-29 09:18:11.41415	ChannelMessage	15	\N	8	f	f	f	T
109	12	<p>Okay, that's a handy feature</p>	2019-08-29 09:19:51.994384	ChannelMessage	15	\N	8	f	f	f	T
111	12	<p>And I saw that we could create / join channels, start conversations</p>	2019-08-29 09:21:55.918053	ChannelMessage	15	\N	8	f	f	f	T
112	11	<p>Yes, you can create channels that everyone can join, and start private conversations with people you invite</p>	2019-08-29 09:22:38.509235	ChannelMessage	15	\N	8	f	f	f	T
113	11	joined & news.	2019-08-29 09:25:01.777334	ChannelMessage	23	\N	8	f	f	t	T
114	11	joined & fun_stuff.	2019-08-29 09:25:36.147417	ChannelMessage	24	\N	8	f	f	t	T
115	11	<p>Also, you could invite other people to join our workspace, through the menu on top right corner</p>	2019-08-29 09:27:38.460618	ChannelMessage	15	\N	8	f	f	f	T
116	11	<p>Through that menu, you can also set your status (e.g. in a meeting)</p>	2019-08-29 09:28:50.824575	ChannelMessage	15	\N	8	f	f	f	T
117	11	<p>by clicking the clog icon on the top, you have options to invite others into the current conversation / channel</p>	2019-08-29 09:34:48.261336	ChannelMessage	15	\N	8	f	f	f	T
118	11	<p>and you can display all members in current conversation / channel</p>	2019-08-29 09:35:17.800228	ChannelMessage	15	\N	8	f	f	f	T
119	11	<p>and you can choose to leave current conversation / channel (except default general / random)</p>	2019-08-29 09:35:46.564684	ChannelMessage	15	\N	8	f	f	f	T
120	11	<p>how are you doing?</p>	2019-08-29 09:38:22.289676	ConversationMessage	\N	14	8	f	f	f	T
121	12	<p>That's good to know!</p>	2019-08-29 09:47:37.959333	ChannelMessage	15	\N	8	f	f	f	T
122	12	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 74.49524940617577%;background-size:5362.5% 5362.5%" data-codepoints="1f60e"></span></span></span></span>锘?/span> </p>	2019-08-29 09:47:57.62545	ChannelMessage	15	\N	8	f	f	f	T
123	12	<p>I like the emojis <span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 54.89904988123515%;background-size:5362.5% 5362.5%" data-codepoints="1f604"></span></span></span></span>锘?/span> </p>	2019-08-29 09:48:21.394239	ChannelMessage	15	\N	8	f	f	f	T
124	11	<p>They're fun, aren't they!</p>	2019-08-29 09:49:11.182638	ChannelMessage	15	\N	8	f	f	f	T
125	11	<p>Don't know you notice or not, when there're many messages in the channel/conversation history, it would only display those recent messages, and loads more when you scroll to top</p>	2019-08-29 09:50:23.352681	ChannelMessage	15	\N	8	f	f	f	T
126	12	<p>Okay, that's cool</p>	2019-08-29 09:50:53.285786	ChannelMessage	15	\N	8	f	f	f	T
127	12	<p>That would definitely help reduce the loading time</p>	2019-08-29 09:51:10.954252	ChannelMessage	15	\N	8	f	f	f	T
128	11	<p>Yep</p>	2019-08-29 10:10:19.3151	ChannelMessage	15	\N	8	f	f	f	T
129	11	<p>Those above are the main features. </p>	2019-08-29 10:10:33.876801	ChannelMessage	15	\N	8	f	f	f	T
130	11	<p>Explore more for yourself now!</p>	2019-08-29 10:10:42.122109	ChannelMessage	15	\N	8	f	f	f	T
131	12	<p>i'm good you?</p>	2019-08-29 10:13:04.712705	ConversationMessage	\N	14	8	f	f	f	T
132	11	<p>fine thanks</p>	2019-08-29 10:13:29.821742	ConversationMessage	\N	14	8	f	f	f	T
133	11	<p>finding everything ok so far?</p>	2019-08-29 10:13:38.46625	ConversationMessage	\N	14	8	f	f	f	T
134	12	<p>yep</p>	2019-08-29 10:13:46.83022	ConversationMessage	\N	14	8	f	f	f	T
135	12	<p>so far so good</p>	2019-08-29 10:13:51.228693	ConversationMessage	\N	14	8	f	f	f	T
136	12	<p>I'm exploring the web</p>	2019-08-29 10:14:10.280381	ConversationMessage	\N	14	8	f	f	f	T
137	12	<p>I like it</p>	2019-08-29 10:15:53.75372	ConversationMessage	\N	14	8	f	f	f	T
138	11	<p>Good to hear that</p>	2019-08-29 10:23:26.178795	ConversationMessage	\N	14	8	f	f	f	T
139	12	<p>thanks!</p>	2019-08-29 10:24:30.510178	ConversationMessage	\N	14	8	f	f	f	T
140	12	<p>Found that you could share web link like this</p>	2019-08-29 10:38:06.656562	ChannelMessage	15	\N	8	f	f	f	T
141	12	<p><a href="https://www.stuff.co.nz/business/115367399/aucklands-new-megamall-opens-its-first-stage-to-eager-shoppers" target="_blank">https://www.stuff.co.nz/business/115367399/aucklands-new-megamall-opens-its-first-stage-to-eager-shoppers</a></p>	2019-08-29 10:38:08.258037	ChannelMessage	15	\N	8	f	f	f	T
142	11	<p>Yep, that's one of the features that I didn't mention</p>	2019-08-29 10:40:07.217169	ChannelMessage	15	\N	8	f	f	f	T
143	11	<p>Good find!</p>	2019-08-29 10:40:30.292526	ChannelMessage	15	\N	8	f	f	f	T
144	12	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 54.89904988123515%;background-size:5362.5% 5362.5%" data-codepoints="1f604"></span></span></span></span>锘?/span> </p>	2019-08-29 10:40:53.975756	ChannelMessage	15	\N	8	f	f	f	T
145	11	<p>See if you can find more!</p>	2019-08-29 10:42:24.609769	ChannelMessage	15	\N	8	f	f	f	T
146	12	<p>sure</p>	2019-08-29 10:42:37.919078	ChannelMessage	15	\N	8	f	f	f	T
147	5	<p>This channel is another default channel (apart from &amp;general) for new users</p>	2019-09-02 02:29:47.715076	ChannelMessage	16	\N	8	f	f	f	T
148	5	joined & weather.	2019-09-02 02:31:00.46027	ChannelMessage	25	\N	8	f	f	t	T
149	5	joined & carpool.	2019-09-02 02:31:13.757182	ChannelMessage	26	\N	8	f	f	t	T
150	5	joined & activities.	2019-09-02 02:31:25.995799	ChannelMessage	27	\N	8	f	f	t	T
151	5	joined & sports.	2019-09-02 02:31:33.282931	ChannelMessage	28	\N	8	f	f	t	T
152	13	joined & general.	2019-09-02 02:37:35.152995	ChannelMessage	15	\N	8	f	f	t	T
153	13	joined & random.	2019-09-02 02:37:35.185997	ChannelMessage	16	\N	8	f	f	t	T
154	14	joined & general.	2019-09-02 02:48:05.646378	ChannelMessage	15	\N	8	f	f	t	T
155	14	joined & random.	2019-09-02 02:48:05.670686	ChannelMessage	16	\N	8	f	f	t	T
156	13	joined & fun_stuff.	2019-09-02 02:50:39.006572	ChannelMessage	24	\N	8	f	f	t	T
157	13	joined & activities.	2019-09-02 02:50:43.183085	ChannelMessage	27	\N	8	f	f	t	T
158	13	joined & sports.	2019-09-02 02:50:49.230657	ChannelMessage	28	\N	8	f	f	t	T
159	13	joined & carpool.	2019-09-02 02:51:02.121237	ChannelMessage	26	\N	8	f	f	t	T
160	14	joined & carpool.	2019-09-02 02:51:09.989163	ChannelMessage	26	\N	8	f	f	t	T
161	14	joined & news.	2019-09-02 02:51:13.841259	ChannelMessage	23	\N	8	f	f	t	T
162	14	<p>Hi man!</p>	2019-09-02 02:51:34.84263	ConversationMessage	\N	17	8	f	f	f	T
163	14	<p>I got the invite and just joined!</p>	2019-09-02 02:51:59.384621	ConversationMessage	\N	17	8	f	f	f	T
164	11	joined & carpool.	2019-09-02 02:52:18.486837	ChannelMessage	26	\N	8	f	f	t	T
165	15	joined & general.	2019-09-02 04:11:44.917929	ChannelMessage	15	\N	8	f	f	t	T
166	15	joined & random.	2019-09-02 04:11:44.94385	ChannelMessage	16	\N	8	f	f	t	T
167	13	<p>Hey bro</p>	2019-09-02 04:12:52.425513	ConversationMessage	\N	19	8	f	f	f	T
168	15	<p>how you guys doing?</p>	2019-09-02 04:13:51.072402	ConversationMessage	\N	20	8	f	f	f	T
169	15	<p>1.\tanalyze</p><p>2. fix</p><p>3. submit</p>	2019-09-02 04:15:57.534378	ConversationMessage	\N	18	8	f	f	f	T
170	15	<p><span class="span-tag">锘?span contenteditable="false"><span data-user-id="12" class="mentioned-user">@User C</span></span>锘?/span> are you here?</p>	2019-09-02 04:16:25.754886	ConversationMessage	\N	20	8	f	f	f	T
171	12	<p>Yeah</p>	2019-09-02 04:18:46.322602	ConversationMessage	\N	20	8	f	f	f	T
172	12	<p>I'm good, thanks</p>	2019-09-02 04:18:52.824055	ConversationMessage	\N	20	8	f	f	f	T
173	12	<p>Welcome everyone!</p>	2019-09-02 04:19:13.010041	ChannelMessage	15	\N	8	f	f	f	T
174	12	<p><span class="span-tag">锘?span contenteditable="false"><span data-user-id="13" class="mentioned-user">@Demo User</span></span>锘?/span> <span class="span-tag">锘?span contenteditable="false"><span data-user-id="14" class="mentioned-user">@Demo UserB</span></span>锘?/span> <span class="span-tag">锘?span contenteditable="false"><span data-user-id="15" class="mentioned-user">@Test User</span></span>锘?/span> </p>	2019-09-02 04:19:26.054223	ChannelMessage	15	\N	8	f	f	f	T
175	15	joined & carpool.	2019-09-02 04:20:25.688037	ChannelMessage	26	\N	8	f	f	t	T
176	15	joined & activities.	2019-09-02 04:20:29.951168	ChannelMessage	27	\N	8	f	f	t	T
177	15	joined & sports.	2019-09-02 04:20:35.437575	ChannelMessage	28	\N	8	f	f	t	T
178	15	joined & fun_stuff.	2019-09-02 04:20:39.406622	ChannelMessage	24	\N	8	f	f	t	T
179	13	joined conversation along with Demo UserB on the invitation of Test User.	2019-09-02 04:21:29.136407	ConversationMessage	\N	22	8	f	f	t	T
180	15	<p>We can share carpool info here</p>	2019-09-02 04:23:20.157325	ChannelMessage	26	\N	8	f	f	f	T
181	15	<p>Cool</p>	2019-09-02 04:23:24.227173	ChannelMessage	26	\N	8	f	f	f	T
182	13	<p>Yep, handy <span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 47.06057007125891%;background-size:5362.5% 5362.5%" data-codepoints="1f600"></span></span></span></span>锘?/span> </p>	2019-09-02 04:24:14.493013	ChannelMessage	26	\N	8	f	f	f	T
183	12	joined & carpool.	2019-09-02 04:25:13.415605	ChannelMessage	26	\N	8	f	f	t	T
184	12	<p>I'd like to share cars too!</p>	2019-09-02 04:26:11.084575	ChannelMessage	26	\N	8	f	f	f	T
185	12	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 74.49524940617577%;background-size:5362.5% 5362.5%" data-codepoints="1f60e"></span></span></span></span>锘?/span> </p>	2019-09-02 04:26:27.905127	ChannelMessage	26	\N	8	f	f	f	T
189	15	<p><span class="span-tag">锘?span contenteditable="false"><span data-user-id="12" class="mentioned-user">@User C</span></span>锘?/span> Sure thing!</p>	2019-09-02 04:29:02.393014	ChannelMessage	26	\N	8	f	f	f	T
190	13	<p>Anyone following rugby?</p>	2019-09-02 04:29:49.922928	ChannelMessage	28	\N	8	f	f	f	T
191	12	joined & sports.	2019-09-02 04:33:04.770601	ChannelMessage	28	\N	8	f	f	t	T
192	12	<p><span class="span-tag">锘?span contenteditable="false"><span data-user-id="13" class="mentioned-user">@Demo User</span></span>锘?/span> Yep I am</p>	2019-09-02 04:33:22.616644	ChannelMessage	28	\N	8	f	f	f	T
193	12	<p>so is <span class="span-tag">锘?span contenteditable="false"><span data-user-id="15" class="mentioned-user">@Test User</span></span>锘?/span> </p>	2019-09-02 04:34:29.999131	ChannelMessage	28	\N	8	f	f	f	T
194	15	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 47.06057007125891%;background-size:5362.5% 5362.5%" data-codepoints="1f600"></span></span></span></span>锘?/span> </p>	2019-09-03 10:19:41.760632	ConversationMessage	\N	26	8	f	f	f	T
195	15		2019-09-04 10:36:37.163148	ConversationMessage	\N	26	8	t	f	f	T
196	15	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 50.979809976247026%;background-size:5362.5% 5362.5%" data-codepoints="1f602"></span></span></span></span>锘?/span> </p>	2020-01-04 02:38:57.224657	ConversationMessage	\N	19	8	f	f	f	T
197	5	<p>123</p>	2020-02-25 23:01:21.832144	ConversationMessage	\N	5	8	f	f	f	T
198	5	<p>123</p>	2020-02-25 23:29:01.146719	ConversationMessage	\N	5	8	f	f	f	T
199	5	<p>aaa</p>	2020-02-25 23:58:01.224175	ConversationMessage	\N	5	8	f	f	f	T
200	5	<p>bbb</p>	2020-02-26 00:00:22.644919	ConversationMessage	\N	5	8	f	f	f	T
203	16	joined & general.	2020-10-10 14:51:33.229683	ChannelMessage	15	\N	8	f	f	t	T
204	17	joined & general.	2020-10-10 14:51:33.229686	ChannelMessage	15	\N	8	f	f	t	T
205	16	joined & random.	2020-10-10 14:51:34.123634	ChannelMessage	16	\N	8	f	f	t	T
206	17	joined & random.	2020-10-10 14:51:34.123566	ChannelMessage	16	\N	8	f	f	t	T
207	18	joined & general.	2020-10-10 14:54:45.022056	ChannelMessage	5	\N	5	f	f	t	T
208	18	joined & random.	2020-10-10 14:54:45.693099	ChannelMessage	6	\N	5	f	f	t	T
209	19	joined & general.	2020-10-10 02:07:13.083436	ChannelMessage	29	\N	13	f	f	t	T
210	19	joined & random.	2020-10-10 02:07:13.181059	ChannelMessage	30	\N	13	f	f	t	T
211	20	joined & general.	2020-10-10 02:08:30.665919	ChannelMessage	17	\N	9	f	f	t	T
212	20	joined & random.	2020-10-10 02:08:30.686771	ChannelMessage	18	\N	9	f	f	t	T
213	6	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:58.818289786223275% 58.818289786223275%;background-size:5362.5% 5362.5%" data-codepoints="1f606"></span></span></span></span>锘?/span> </p>	2020-10-10 02:09:33.531048	ConversationMessage	\N	35	9	f	f	f	T
214	21	joined & general.	2020-10-10 02:22:33.859449	ChannelMessage	17	\N	9	f	f	t	T
215	21	joined & random.	2020-10-10 02:22:34.131322	ChannelMessage	18	\N	9	f	f	t	T
216	21	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container"><span class="emoji-outer emoji-sizer"><span class="emoji-inner" style="background: url(https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png);background-position:60.77790973871734% 0.02969121140142518%;background-size:5362.5% 5362.5%" data-codepoints="1f61c"></span></span></span></span>锘?/span> </p>	2020-10-10 02:23:59.1808	ConversationMessage	\N	37	9	f	f	f	T
217	20	<p>鍑哄幓鐜?/p>	2020-10-10 02:24:00.128162	ChannelMessage	18	\N	9	f	f	f	T
218	20	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container">馃惙</span></span>锘?/span> </p>	2020-10-10 02:24:31.32933	ConversationMessage	\N	37	9	f	f	f	T
219	21		2020-10-10 02:24:35.930047	ConversationMessage	\N	37	9	t	f	f	T
220	21	<p>鐪嬪緱鎳傚槢</p>	2020-10-10 02:24:45.597636	ConversationMessage	\N	37	9	f	f	f	T
221	20	<p>neglect</p>	2020-10-10 02:26:30.010634	ConversationMessage	\N	37	9	f	f	f	T
222	20		2020-10-10 02:26:47.653972	ConversationMessage	\N	37	9	t	f	f	T
223	20	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container">馃悥</span></span>锘?/span> </p>	2020-10-10 02:27:02.295562	ConversationMessage	\N	37	9	f	f	f	T
224	20	<p><span class="span-tag">锘?span contenteditable="false"><span class="emoji-container">馃惤</span></span>锘?/span> </p>	2020-10-10 02:27:09.247686	ConversationMessage	\N	37	9	f	f	f	T
225	21	<pre></p><p>can you</p><p></pre>	2020-10-10 02:27:46.57933	ConversationMessage	\N	37	9	f	f	f	T
226	21	<p><a href="https://www.stuff.co.nz/opinion/114832273/jim-hubbard-cartoons" target="_blank">https://www.stuff.co.nz/opinion/114832273/jim-hubbard-cartoons</a></p>	2020-10-10 02:28:32.507229	ConversationMessage	\N	37	9	f	f	f	T
227	20	<p><a href="https://www.youtube.com/watch?v=4VWQdkK455E" target="_blank">https://www.youtube.com/watch?v=4VWQdkK455E</a></p>	2020-10-10 02:29:08.064413	ConversationMessage	\N	37	9	f	f	f	T
228	21		2020-10-10 02:30:38.867872	ConversationMessage	\N	38	9	t	f	f	T
229	20	<p>piggy LEILEI</p>	2020-10-10 07:01:17.030321	ConversationMessage	\N	38	9	f	f	f	T
230	20	<p>鐚ご鍢?/p>	2020-11-10 07:54:21.848382	ChannelMessage	18	\N	9	f	f	f	T
231	20	<p>濮戝鎯冲嚭鍘荤帺 鍙槸鐚ご鍦ㄧ爜浠ｇ爜</p>	2020-11-10 07:55:01.693457	ChannelMessage	18	\N	9	f	f	f	T
232	20		2020-11-10 07:57:27.452296	ChannelMessage	18	\N	9	t	f	f	T
\.


--
-- Data for Name: ResetPasswordRequsets; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ResetPasswordRequsets" ("Id", "Email", "Resetted", "Cancelled", "ResetCode", "TRIAL662") FROM stdin;
1	demo_ichat@gmail.com	f	f	c7a820d6-9c1e-42aa-9dcf-8dd1a9af1ad0	T
2	liefu.zhang@vocusgroup.co.nz	t	f	d8cb9fa6-8ede-4d3a-93e1-67d297c32a0b	T
\.


--
-- Data for Name: UserInvitations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserInvitations" ("Id", "UserEmail", "InvitedByUserId", "WorkspaceId", "InviteDate", "Acceptted", "InvitationCode", "Cancelled", "TRIAL666") FROM stdin;
1	ichat.user.c@gmail.com	7	10	2019-08-12 09:26:03.161383	f	655861bd-4d69-46b3-b381-61d1ae5a3b12	t	T
2	ichat.user.c@gmail.com	7	10	2019-08-12 09:26:09.819247	f	dee8861f-fc86-425c-a006-833179154a20	t	T
3	ichat.user.c@gmail.com	7	10	2019-08-12 09:26:15.071687	f	f08ba380-78f8-4624-a403-99142700ac0d	t	T
4	ichat.user.c@gmail.com	7	10	2019-08-12 09:26:45.653426	f	f9a6d75c-4a22-460a-906b-134fa3390496	t	T
5	ichat.user.c@gmail.com	7	10	2019-08-12 09:28:19.914656	f	62c5e42f-ef65-4bd6-9f6b-24008524ea09	t	T
6	ichat.user.c@gmail.com	7	10	2019-08-12 09:28:43.607527	f	19e19967-f66c-4831-be97-3efefc505639	t	T
7	ichat.user.c@gmail.com	7	10	2019-08-12 09:37:46.809511	f	695557c5-4f18-4ef9-af51-0b1fd9551acc	f	T
8	ichat.user.c@gmail.com	8	11	2019-08-12 09:42:11.792337	f	eaebd3c6-cd56-4472-baab-0a1ef7e4dd3c	t	T
9	ichat.user.c@gmail.com	8	11	2019-08-12 09:45:16.419452	t	9237b9a0-d118-4027-a8e1-54daf858964e	f	T
10	ichat.user.b@gmail.com	8	11	2019-08-12 09:48:19.907658	f	5847aeca-3b2b-4fee-9dc1-3ae8afa42ebf	t	T
11	ichat.user.b@gmail.com	8	11	2019-08-12 09:48:57.941421	t	669944d5-5d7b-4cd8-9656-c88087833eb7	f	T
12	liefuzhang@163.com	5	8	2019-08-27 04:02:05.928034	t	b84f3c3b-a23e-4470-a6b1-145bbebf7e73	f	T
13	liefu.zhang@vocusgroup.co.nz	5	8	2019-08-27 04:02:11.155659	t	d3ac1699-3e4f-421b-ad88-a2b344eb39ef	f	T
14	ichat.demo.user@gmail.com	5	8	2019-09-02 02:36:50.471679	t	2cde0fd3-3c17-4dfa-99ce-43215e03916e	f	T
15	ichat.demo.user2@gmail.com	13	8	2019-09-02 02:46:55.911751	t	754b0532-fd44-4c81-967d-05b0966046d0	f	T
16	ichat.test.user@gmail.com	14	8	2019-09-02 04:11:17.301018	t	dd95e222-501c-41ac-bb53-a593f0a85aef	f	T
17	ellawu533@gmail.com	6	9	2020-10-10 00:27:40.130131	f	4e5cf0f7-0952-41ca-830a-90d00d9a27ee	t	T
18	ellawu533@gmail.com	6	9	2020-10-10 00:27:46.045783	f	52c1400a-7cbd-4e4e-b836-a98b9cac6f79	t	T
19	ellawu533@gmail.com	6	9	2020-10-10 00:27:50.882826	f	af0d58bf-3120-4054-b9e9-b4b850c7619f	t	T
20	liefuzhangnz@gmail.com	6	9	2020-10-10 00:34:43.861215	f	dcf865c9-f57c-4957-9189-55f2803f654b	t	T
21	liefuzhangnz@gmail.com	6	9	2020-10-10 00:40:53.599167	f	e52faaa4-9604-4565-a292-4dbcfc945255	t	T
22	liefuzhangnz@gmail.com	6	9	2020-10-10 00:42:22.267791	f	02c07ff8-cc11-42e8-9e71-4cd788771416	t	T
23	liefuzhangnz@gmail.com	6	9	2020-10-10 13:47:30.315126	f	eb11b52b-6e0b-4e6b-901b-ae4fae76f3be	t	T
24	liefuzhangnz@gmail.com	6	9	2020-10-10 13:50:12.600908	f	a9e1151d-3bc0-4400-9dc6-b45014dc6531	t	T
25	liefuzhangnz@gmail.com	6	9	2020-10-10 13:58:45.74264	f	e718579d-0e3c-4aa3-b652-3316b8278bd5	t	T
26	liefuzhangnz@gmail.com	6	9	2020-10-10 14:19:31.476383	f	b65ab4cb-4cf8-4884-bb8c-0b73a973c9ca	t	T
27	liefuzhangnz@gmail.com	6	9	2020-10-10 01:21:17.932481	f	b438f404-7638-4262-990f-fd6201d8e08f	f	T
28	liefuzhangnz@gmail.com	15	8	2020-10-10 01:22:42.175164	f	6a1ae75d-9738-4073-abf1-09ccd9c1e5a2	t	T
29	liefuzhangnz@gmail.com	15	8	2020-10-10 01:24:12.5861	f	ab2f8ef3-377f-44ff-8297-65b7d49bcc6b	t	T
30	liefuzhangnz@gmail.com	15	8	2020-10-10 01:24:52.828497	f	f703ef16-4f42-461f-8b13-3b3263039ee2	t	T
31	liefuzhangnz@gmail.com	15	8	2020-10-10 01:28:50.641762	f	6c5cea7f-6ae7-457c-9d4c-b3995f37109c	t	T
32	liefuzhangnz@gmail.com	15	8	2020-10-10 01:29:26.020758	f	664637d3-f9bf-4f11-9bb2-04007fe48602	t	T
33	liefuzhangnz@gmail.com	15	8	2020-10-10 01:36:21.362801	t	e50f0557-7c96-4fb2-b154-f82c0e39a61b	f	T
34	ellawu533@gmail.com	6	9	2020-10-10 01:38:01.503757	t	3cb3432c-63b7-4d76-99ab-45721e468a0f	f	T
36	leifzhang23@gmail.com	3	5	2020-10-10 14:53:13.884484	f	6fed4eb8-7bab-4c41-83e3-c83423e66d52	t	T
37	leifzhang23@gmail.com	3	5	2020-10-10 14:53:14.393548	t	36869f9e-7054-4204-9e2a-34b6aee20f99	f	T
38	157034068@qq.com	6	9	2020-10-10 02:21:42.586361	t	39566391-5159-498a-a898-976dec190953	f	T
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Email", "Status", "DisplayName", "PasswordHash", "PasswordSalt", "WorkspaceId", "CreatedDate", "IdenticonGuid", "PhoneNumber", "TRIAL669") FROM stdin;
2	zzz@test.com	0	zzz	\\x79336977bb72041ab791973a1ea4aba6085c2c2edd691740a646a17a3fb34319cedc6d1a5e5a13cdb6eea9fec6d6f960d1f2327c9e3be387630f4dc0ccaf8f28	\\x65dbc2a2ee296db89a2ec4a7e3df6173c3771fbceef8bb8d12677e342eb7267c34653fef4e2067ba33b53921bf3e5e7ac28c3f884ce04633b4749fd6a8597f885c15282421f9e7ea60811882145b84ec9dcd4f8f2cec2195c53b15625522043ec163724213f9e6e3b21666a8b3d783cc9c53808c514cc41f3b951f5c668744c7	3	2019-08-04 05:23:25.075455	237228e8-68db-4bb6-9d4d-c378496b37a2	\N	T
3	test@test.com	0	123123	\\x14f1d94f77c5622e8e4e988ca270cb5ebebda5eb6a1419c682e72a812294ca1ce126d39800e17fce0f6e1f8c244c3dac28a03c247330c1b43a5c73301053665a	\\xd43dce0609bdba8d1750675167891a7e89770748290992dbbdcea3922c51116cc5a6e3819c340b6490f70aab86a714b0cbb6da8e80baeba803b23cccce7a0493f56d52f50b63a47341daf882f0b2daa99d077f3a480a7ce965784fe1a65f321bf53149d9a52de83a9731510d5bee62c2a515263af5f358f413cd47c5b560e20f	5	2019-08-04 11:40:50.668537	b75a53d9-e277-43f5-9250-a7ca36f9ad53	\N	T
4	test@test.com1	0	test1	\\x432664302df87db72937bb678feda73aef2eca31ebc2eda152e136ed3aed23c5eb0c3f1b45592ad306f1b18f1aee7edef0108cb3c3443f83db31e2c0c209786f	\\x39d1d9a3bbf4049c7b31fbbaa64a6ab46abeaf1917a824acc5e783312cb5fe07373fdf338af19f1dab8b408de11891ef33e353cc6fc1455680060c60e9085b98f38084d9179e75551d3e48f87175426ed84a38ea5d7bc4b59817ebd9dc6b1dea86cebb749d974f650185cdacfcaf30bb7e35d7d4c5599c0f94adfd9599bb3782	7	2019-08-06 07:42:21.951148	a1d5e3fb-cd21-4465-8586-1538d0786ea3	\N	T
5	demo_ichat@gmail.com	0	User A	\\xe1f2d7b7f746f7be3c0f5c5cc85be878c53daa3f5b43fae245b07d3bdadfe3ef06247aaf98e45d1e639aa609766aa38968c630cb3ed1f4bd34d38317ead157	\\xf513b6776637c6423cd6ea352c96f19687cf343715d7386e6f2bf44a9c3f7f84ff6a278205298203f04f39f2b4ee2743f23c150ffc7bed6821909f1cd09f6282fd9b6fb34be7514e09a812df11d6b049409fc7f6648a33398762086583ad3259f166671fcc8cd443f782968cb45045983a821bc85feda61d5f7df7deb1471d	8	2019-08-06 23:18:05.342761	bb7edfc4-4492-4a3e-9ea9-afa42ef24026	092203323	T
6	ichat_usera@gmail.com	0	User A	\\xf94fe3b57d10e4076c6ef00263dd9f0d2f1a65b50e8a27ee715efeb4c09fd2220ad23402e43d62f7619a0cb4aa30717dbc095eba9631f423af45494c01b33cc6	\\xd4f6458a7154b791d1040381acf68169c923c322fcb5a2065aef3a1a00003072fce047f7899b03526576832b33531655dead3eaa82426c4ffa01359819a21b2351c026336fa85c523ac0f24805676dbdc4c5ae013c9f679e0f11067d249d8a39593ea9e6197e0188efdcdc74ff43aeb9b383e0aa8d4239a484e93f1906051603	9	2019-08-12 09:16:02.672694	19069534-6027-4972-b17a-cc11318c998d	\N	T
7	ichat_user_a@gmail.com	0	User A	\\x1fb5e5c5fe0efbf7695f47005804ff8416a91eb1bbd335f234e68fb5427ad73f50010155e911d15a3725cfd93907b3e84474123cded1741eddf896d4b4a3abfc	\\xb0b586d400e680ad983ca9fed559761fbfe5706715d85e0287ae9bdf0fc55fa69f58e8a34f9c9bacc2028910c9b767ebb51078521d06a76cd838f04c3ee5105752be380c1d6e73cf2d468f6d8d56fbf60fb2b8995aad41536a92f67517b29fc98d60ffd905df6bcd6b22a8c5795addc28c6dea28ace544187f64d1bff5cfa070	10	2019-08-12 09:20:22.306439	428b6e7b-6fd6-4d7f-b187-b03dd187b75d	\N	T
8	ichat.user.a@gmail.com	0	User A	\\xe5a913495f800ae860d84621f4c39e97cbd965cbf07b2a1ed42a985b3ad4d7ec3c50e5bd8970c40aaaa9ed8b04b745a716429c4c1b21a20ec43ae155debbc381	\\x59c12d3a174d73da062e9b3f029745b18ba5a40b685700e683fff1c39f7d12212e17f3d294a3e8d74d48781f9e026ed753a126e00c6a99b5ea0f042b043010b2fdb386e444df1139704e0d2b85acdb4f8843e385cd86889d457645e9d70caebe548f6bfe7e654a66600128febdb14003cc2bb6c010ca5f8551ac9bb2555182a8	11	2019-08-12 09:41:55.965701	c2cb3145-b169-46a4-bb7f-fcb2fd8ac56f	\N	T
9	ichat.user.c@gmail.com	0	User C	\\x2b078443b308b3961c283549329359b430a26c985a46eabeaf850673997ada8a58487d1105e7288ace77eb53b1311f2f6dddcff4535334512db7c45790cfea34	\\xa562e1ef05639b0a319942183729955bbbac97a430aeb1c4fd7cdc5f88c295a113234770229249b90b3865d663381bbac1ca632a4916d1c5f3149456b705aeb2c30674caf6f005b02a52cf755e6ac94c3e79f26997ae20ffea3b633e21bec2aa62a55a378fe14e54c8b6c63196c947452f7faf9789ad0f6209c917aa0b702694	11	2019-08-12 09:45:52.936526	4fd6033c-466d-47be-a666-97cd937f7212	\N	T
10	ichat.user.b@gmail.com	0	User B	\\xa77693098a959d0ecf8dec6eb4521990299b11b7a866d3642fbc75aff0a45c08048df7c1c95368c27954eb94624a227a2fc19c3fd60e37957f122914a86f5ee9	\\xdec6f24569516ef73dfbd280aac54902c7f314aa7e751a0558ff90df64fc5b915013c9bc982427f1520a0b944bc32bc808d3f2ba605560be790d329e3ee32921c96cbd42d85f87948c2ce111fcb569d5337e76ddf99c68aa08c5b3427a407eb746827ba2cc713911c9a24d4c20e275c76c05987d94bfaf3c939c52fc56c44ec1	11	2019-08-12 09:50:53.355557	27067465-a7ab-430f-ab3a-e85aead0e369	\N	T
11	liefuzhang@163.com	0	User B	\\x95d4b25b771ccafc3f70b4ebfc056df33d56aba30473f14e59004f228c3b03b7098768eb7b0d9ab73f1fed4703e54e1a603b81bae3fef79ed477ecd85a5770ea	\\x7fa7e655786e8febd05e197c214c5b170fac8a3afcd8c8f2874bbc290d03053791b991ecde02e3aeae45ff6113412ba93bfa9c7ff5c270d0a7f3e906a6c270c16d6474a43b37470c5f9d15494d789c2e1fdfcdec3e97b6d2540d73d2b858c9a8ad921db50bb8814cffd5ba04bb1672faf871e6219abaeb373a6857e4951ed075	8	2019-08-27 04:06:45.73005	f06caf8d-15a9-45fd-a590-30cc567fa867	\N	T
12	liefu.zhang@vocusgroup.co.nz	0	User C	\\x25900b1168036bb24fe67c5f569732ba2ebf5804259cd00a53efb274098bbc40e7b380c9cdb10151eed65e36d34c5d056672b6a9546a5597a0c18b33f70a8095	\\x0e159d591393bc1728b088eb31b21872abf7c4354ff81461e7e1f7c9a38040e48a5a9073856684d0c53cd7d6a77968c7a55bac5963786e5df00877da921ba9261eb4a8a6b8067b30655b4ca69bdbe92afaeeb3b9c11c373dcdf031e3e7314021108d8fc6e6eebc0f42d656e0a344fd09e28793963d0ebab612249ad504bec5fd	8	2019-08-27 04:07:22.83526	84a1c341-94ac-4e51-9c89-572a5b7febfb	\N	T
13	ichat.demo.user@gmail.com	0	Demo User	\\x778473b03609564e4d8bad003c819a20c8af2c31859e20eee27c1f4eb28554fea33ed4eb7465fffa5a33fc6052d0b14cf0890880222cb1126a9d83cc2121639b	\\x1252470b468185c9126982dc804c322b14ac4e032f3c9791195d738dce6eb0acda2aac5e97aea443ba3d3d5dd367f4684f67c4f6b02fd06f3d80635df093b35c4b371cf4ac3aae131a2a6ff8b554d12ec268598351efbadf0b4a9f986a34cf02224120b3cb95795dc111a3d55780dfa76ba04cba8b918808a613b266fab22c66	8	2019-09-02 02:37:35.09709	941e8c99-a296-42c8-a6fb-a62c7ac08863	\N	T
14	ichat.demo.user2@gmail.com	0	Demo UserB	\\x8269211a658adf422b69cd7d501a4dd327457cbc415db0fd49dd438da5acb77ece488902acecf1f7ccc76d28c099002c02956b29c60dfee75b888b911538d3c2	\\xe71ecbeaccd336f420e7cdb19b14a325831d2a418b2b694986024f88e8ab8a012776eaeaf3fa1f9b4cbba4543a7ba29da38794707204a83122e26be2efe731ef4c02fe56569ad477b70254852935f1e819e3c08a4334b40709a6443a92193f7721b7f80c20f194d890cea17132c6e0661a32704aae1245c2a56335bcf5dd53f0	8	2019-09-02 02:48:05.608888	7150ce99-8a2f-4a6f-8328-25ab250efc73	\N	T
15	ichat.test.user@gmail.com	0	Test User	\\xf8e1f2d7b7f746f7be3c0f5c5cc85be878c53daa3f5b43fae245b07d3bdadfe3ef06247aaf98e45d1e639aa609766aa38968c630cb3ed1f4bd34d38317ead157	\\x19f513b6776637c6423cd6ea352c96f19687cf343715d7386e6f2bf44a9c3f7f84ff6a278205298203f04f39f2b4ee2743f23c150ffc7bed6821909f1cd09f6282fd9b6fb34be7514e09a812df11d6b049409fc7f6648a33398762086583ad3259f166671fcc8cd443f782968cb45045983a821bc85feda61d5f7df7deb1471d	8	2019-09-02 04:11:44.884469	b8e344ba-8d2a-413a-bf01-6898de0551f1	\N	T
16	liefuzhangnz@gmail.com	0	dsaom	\\x246c1de12f79954edbbe7f14f6b3b5fd0e7d27e218ff323c02ef01383e3b9932151ca3b18ff325ab3a7d88f69eef62f23bfc67382cf42532c6f9135cdfa67230	\\x665516e15e78ac713f77269442c29e87e5a8e717fb668272775d713cd8bbe87931ef8a359fa940eac68c63f72184771f71656de491e1973321cf3db6672948c4a6263c6058ef2991caab40ff96cde61a5c28f1ff49a77d1442f4d933cc459cd793a70258829b16ee1cc7c1892a00e145ab0bb29b93d04705a4c8a372a22a4516	8	2020-10-10 14:51:30.040436	664a1d4c-f3d5-4b6c-9642-697662d06d51	\N	T
17	liefuzhangnz@gmail.com	0	liefu	\\xc3acf2a96f22ef740b0f9cdbfbc09a2fc22c48d8988328ecea9514ba6a7a6192dc849438d7a68dd9509b530c13c9ae8ab1d44b6921edbb09863156b860ede695	\\x1efda1f8ca0d8fa99d07c8fd8e652c991614dbde505e0981420699980dbe5dc514a2f4cb819fb41a3833d9dce460562511b9638836c18c927b61bea72481ca19ac70c607e6d3bc9fb17ec21dd7fd1bb38ea3841577388cc983d8e53c865744fc5cc7be53d0c26c77b90a07b8d78f577bd882e9a9737ab9a6d0f834c3f0b4a5d6	8	2020-10-10 14:51:29.101371	16d27958-f7fd-4b5a-8acf-d57c2171cdfc	\N	T
18	leifzhang23@gmail.com	0	test	\\xb926b0c7c0154dd90839e525ed30bf9be8b3e171f96925856cbca65ce9ac8a2b5e12ce1cb9215585edf38aa87ed8ba359e9f173929e6216c6ffa70894e2e0b2c	\\x618ed5fa7c46f84eb25fde53c019655612a4d5d413ccde1d5d656468d2385e14ca0784f6b84f6e96cb93380eb448931e4a365a2f3c0a7ec019d48a63db32cac0f7eea793e41d64fa194971b695ba5ac4e40d3f8f4b1be02cd2522af8abea72d06cc9d13ea9d56a55fc789834d8d391f0c8e60d1946e74e0159c5c0b172089c80	5	2020-10-10 14:54:42.865958	785d7cc2-53f6-4678-8822-ecfcca4d5e0e	\N	T
19	somenew@test.com	0	Liefu	\\xead6a07165209edd621c13be211e953370daa4b590bed1e9f827a14897f227129e5f2b7a50448a40864760d684e8325c9f28be1d11797f49b1deac8c49a2e30c	\\x5d70883841ae067a8d5511db489c21c65664bc8793917bc27f06d3c1ca094ffbb859d3188c2a2648743b81bf04cd33ed6f9b66074b1d2510d77b4275c1af7fcd4e2d5f7e5b38821bae16687714f681bc9aaa9f12be99bd5fd230e3cfa249893af6a4f665addc6c2ce7423a68b80c3381706f6d3ab80ac4b40ef1ce521460b100	13	2020-10-10 02:07:12.77183	d70bd46f-08ac-46c8-a912-dc04cfe4b9ad	\N	T
20	ellawu533@gmail.com	0	Shanshan	\\x21dc1fc4ff46e92f827f4966111643d92d25a965c2bf66361fb216fe4927294c9399a31551a5b05d68a175bde85833f113a7acae59617fd78bd9fbf02b28a514	\\x51ead8a014239492bd0b1134f0f6bab42fdd5a039d9b8d5f61003536013d825344af673201bbd598232c4da2c75f9159e2bbf956e2ea88ded4e44bfc17e209b4ec689e8a6149cc03791c6b5cfd6259b4ad706da008fa7424caa12ee3ff7a4859cc623ecb6b96d8c7ec047d94e221b48c5176e0338c4b10bc6c4c178a66344f95	9	2020-10-10 02:08:30.631913	dbd629bf-8437-4875-a54e-20758464dad5	\N	T
21	157034068@qq.com	0	Leif	\\x7cb1d034c6fcedcf14676719dbccf5117215ab84555f8e8c3438aae11cc8b195aaf29408467e22d2ee5a4c2a375e117785a1d26daedc9fea6fbf2518ba3be735	\\xaf6d60c0ba718cb2b4d3cbbeda96267aa88730c60b4f841ec492ccc4b8449b15b6ec1a11f4603eb62ed0768d37253a67d48d6614baa4fcc20d04aad24be1bdda7c5b09ab4958f774f2925594c611a01b5fced59f8ec0c9bd28ab540889daa6f238ab438973f8d0f8f9be7c4be35039468f4d23c12dfbcd308f4c3a7c48608e1f	9	2020-10-10 02:22:33.726104	90da0059-1981-4079-83b5-c62df2578d66	\N	T
\.


--
-- Data for Name: Workspaces; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Workspaces" ("Id", "Name", "OwnerId", "CreatedDate", "TRIAL672") FROM stdin;
1	TestWorkspace	0	2019-08-04 00:34:27.235148	T
2	TestWorkspace1	0	2019-08-04 00:37:48.704934	T
3	workplace7	0	2019-08-04 05:23:24.029953	T
4	spacec	0	2019-08-04 11:39:46.701747	T
5	spaced	0	2019-08-04 11:40:50.64259	T
6	spacex	0	2019-08-06 07:41:02.323345	T
7	spacey	0	2019-08-06 07:42:21.93046	T
8	WorkspaceDemo	0	2019-08-06 23:18:04.875282	T
9	ichat_demo_workspace	0	2019-08-12 09:16:01.850661	T
10	demo_workspace	0	2019-08-12 09:20:22.181695	T
11	demo_company	0	2019-08-12 09:41:55.632086	T
12	new_workspace	0	2020-10-10 02:01:51.610799	T
13	new_workspace1	0	2020-10-10 02:07:11.185076	T
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion", "TRIAL633") FROM stdin;
20190505220841_InitalModel	2.2.2-servicing-10034	T
20190508022344_AddPwdToUser	2.2.2-servicing-10034	T
20190511004928_AddWorkSpaceModel	2.2.2-servicing-10034	T
20190511223226_RenameTabels	2.2.2-servicing-10034	T
20190513093530_AddWorkspaceIdToMessage	2.2.2-servicing-10034	T
20190515220328_AddIdenticonGuidToUser	2.2.2-servicing-10034	T
20190519062942_AddUserInvitation	2.2.2-servicing-10034	T
20190607025116_AddedConversationModels	2.2.2-servicing-10034	T
20190607035528_RenameConversationUserToConversationUsers	2.2.2-servicing-10034	T
20190607112835_RemoveNameFromConversation	2.2.2-servicing-10034	T
20190613091038_AddFileAndFileMessage	2.2.2-servicing-10034	T
20190613094456_RemoveFileMessage	2.2.2-servicing-10034	T
20190613094825_AddMessageHasFileAttachments	2.2.2-servicing-10034	T
20190613233356_AddNameToFile	2.2.2-servicing-10034	T
20190618043850_AddContentTypeToFile	2.2.2-servicing-10034	T
20190619030507_AddPhoneNumberToUser	2.2.2-servicing-10034	T
20190622113810_AddContentEditedToMessage	2.2.2-servicing-10034	T
20190623010920_AddedMessageReactionTables	2.2.2-servicing-10034	T
20190625204402_AddCreatedDateToMessageReaction	2.2.2-servicing-10034	T
20190627090636_AddedResetPasswordRequest	2.2.2-servicing-10034	T
20190630051431_AddedCreatedInfoForConversationAndChannel	2.2.2-servicing-10034	T
20190701103703_AddedIsSystemMessage	2.2.2-servicing-10034	T
20190702040615_AddedIsPrivateAndIsSelfConversation	2.2.2-servicing-10034	T
\.


--
-- Name: Channels_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Channels_Id_seq"', 30, true);


--
-- Name: Conversations_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Conversations_Id_seq"', 38, true);


--
-- Name: Files_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Files_Id_seq"', 9, true);


--
-- Name: MessageReactions_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MessageReactions_Id_seq"', 16, true);


--
-- Name: Messages_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Messages_Id_seq"', 232, true);


--
-- Name: ResetPasswordRequsets_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ResetPasswordRequsets_Id_seq"', 2, true);


--
-- Name: UserInvitations_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserInvitations_Id_seq"', 38, true);


--
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 21, true);


--
-- Name: Workspaces_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Workspaces_Id_seq"', 13, true);


--
-- Name: ChannelSubscriptions PK_ChannelSubscriptions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChannelSubscriptions"
    ADD CONSTRAINT "PK_ChannelSubscriptions" PRIMARY KEY ("ChannelId", "UserId");


--
-- Name: Channels PK_Channels; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Channels"
    ADD CONSTRAINT "PK_Channels" PRIMARY KEY ("Id");


--
-- Name: ConversationUsers PK_ConversationUsers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ConversationUsers"
    ADD CONSTRAINT "PK_ConversationUsers" PRIMARY KEY ("ConversationId", "UserId");


--
-- Name: Conversations PK_Conversations; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Conversations"
    ADD CONSTRAINT "PK_Conversations" PRIMARY KEY ("Id");


--
-- Name: Files PK_Files; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Files"
    ADD CONSTRAINT "PK_Files" PRIMARY KEY ("Id");


--
-- Name: MessageFileAttachments PK_MessageFileAttachments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageFileAttachments"
    ADD CONSTRAINT "PK_MessageFileAttachments" PRIMARY KEY ("FileId", "MessageId");


--
-- Name: MessageReactionUsers PK_MessageReactionUsers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactionUsers"
    ADD CONSTRAINT "PK_MessageReactionUsers" PRIMARY KEY ("MessageReactionId", "UserId");


--
-- Name: MessageReactions PK_MessageReactions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactions"
    ADD CONSTRAINT "PK_MessageReactions" PRIMARY KEY ("Id");


--
-- Name: Messages PK_Messages; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages"
    ADD CONSTRAINT "PK_Messages" PRIMARY KEY ("Id");


--
-- Name: ResetPasswordRequsets PK_ResetPasswordRequsets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ResetPasswordRequsets"
    ADD CONSTRAINT "PK_ResetPasswordRequsets" PRIMARY KEY ("Id");


--
-- Name: UserInvitations PK_UserInvitations; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserInvitations"
    ADD CONSTRAINT "PK_UserInvitations" PRIMARY KEY ("Id");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: Workspaces PK_Workspaces; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Workspaces"
    ADD CONSTRAINT "PK_Workspaces" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_ChannelSubscriptions_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChannelSubscriptions_UserId" ON public."ChannelSubscriptions" USING btree ("UserId");


--
-- Name: IX_Channels_CreatedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Channels_CreatedByUserId" ON public."Channels" USING btree ("CreatedByUserId");


--
-- Name: IX_Channels_WorkspaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Channels_WorkspaceId" ON public."Channels" USING btree ("WorkspaceId");


--
-- Name: IX_ConversationUsers_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ConversationUsers_UserId" ON public."ConversationUsers" USING btree ("UserId");


--
-- Name: IX_Conversations_CreatedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Conversations_CreatedByUserId" ON public."Conversations" USING btree ("CreatedByUserId");


--
-- Name: IX_Conversations_WorkspaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Conversations_WorkspaceId" ON public."Conversations" USING btree ("WorkspaceId");


--
-- Name: IX_Files_UploadedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Files_UploadedByUserId" ON public."Files" USING btree ("UploadedByUserId");


--
-- Name: IX_Files_WorkspaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Files_WorkspaceId" ON public."Files" USING btree ("WorkspaceId");


--
-- Name: IX_MessageFileAttachments_MessageId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_MessageFileAttachments_MessageId" ON public."MessageFileAttachments" USING btree ("MessageId");


--
-- Name: IX_MessageReactionUsers_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_MessageReactionUsers_UserId" ON public."MessageReactionUsers" USING btree ("UserId");


--
-- Name: IX_MessageReactions_MessageId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_MessageReactions_MessageId" ON public."MessageReactions" USING btree ("MessageId");


--
-- Name: IX_Messages_ChannelId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Messages_ChannelId" ON public."Messages" USING btree ("ChannelId");


--
-- Name: IX_Messages_ConversationId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Messages_ConversationId" ON public."Messages" USING btree ("ConversationId");


--
-- Name: IX_Messages_SenderId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Messages_SenderId" ON public."Messages" USING btree ("SenderId");


--
-- Name: IX_Messages_WorkspaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Messages_WorkspaceId" ON public."Messages" USING btree ("WorkspaceId");


--
-- Name: IX_UserInvitations_InvitedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserInvitations_InvitedByUserId" ON public."UserInvitations" USING btree ("InvitedByUserId");


--
-- Name: IX_Users_WorkspaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_WorkspaceId" ON public."Users" USING btree ("WorkspaceId");


--
-- Name: ChannelSubscriptions FK_ChannelSubscriptions_Channels_ChannelId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChannelSubscriptions"
    ADD CONSTRAINT "FK_ChannelSubscriptions_Channels_ChannelId" FOREIGN KEY ("ChannelId") REFERENCES public."Channels"("Id") ON DELETE CASCADE;


--
-- Name: ChannelSubscriptions FK_ChannelSubscriptions_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChannelSubscriptions"
    ADD CONSTRAINT "FK_ChannelSubscriptions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Channels FK_Channels_Users_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Channels"
    ADD CONSTRAINT "FK_Channels_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES public."Users"("Id");


--
-- Name: Channels FK_Channels_Workspaces_WorkspaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Channels"
    ADD CONSTRAINT "FK_Channels_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES public."Workspaces"("Id") ON DELETE CASCADE;


--
-- Name: ConversationUsers FK_ConversationUsers_Conversations_ConversationId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ConversationUsers"
    ADD CONSTRAINT "FK_ConversationUsers_Conversations_ConversationId" FOREIGN KEY ("ConversationId") REFERENCES public."Conversations"("Id") ON DELETE CASCADE;


--
-- Name: ConversationUsers FK_ConversationUsers_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ConversationUsers"
    ADD CONSTRAINT "FK_ConversationUsers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Conversations FK_Conversations_Users_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Conversations"
    ADD CONSTRAINT "FK_Conversations_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES public."Users"("Id");


--
-- Name: Conversations FK_Conversations_Workspaces_WorkspaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Conversations"
    ADD CONSTRAINT "FK_Conversations_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES public."Workspaces"("Id") ON DELETE CASCADE;


--
-- Name: Files FK_Files_Users_UploadedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Files"
    ADD CONSTRAINT "FK_Files_Users_UploadedByUserId" FOREIGN KEY ("UploadedByUserId") REFERENCES public."Users"("Id");


--
-- Name: Files FK_Files_Workspaces_WorkspaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Files"
    ADD CONSTRAINT "FK_Files_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES public."Workspaces"("Id");


--
-- Name: MessageFileAttachments FK_MessageFileAttachments_Files_FileId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageFileAttachments"
    ADD CONSTRAINT "FK_MessageFileAttachments_Files_FileId" FOREIGN KEY ("FileId") REFERENCES public."Files"("Id") ON DELETE CASCADE;


--
-- Name: MessageFileAttachments FK_MessageFileAttachments_Messages_MessageId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageFileAttachments"
    ADD CONSTRAINT "FK_MessageFileAttachments_Messages_MessageId" FOREIGN KEY ("MessageId") REFERENCES public."Messages"("Id") ON DELETE CASCADE;


--
-- Name: MessageReactionUsers FK_MessageReactionUsers_MessageReactions_MessageReactionId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactionUsers"
    ADD CONSTRAINT "FK_MessageReactionUsers_MessageReactions_MessageReactionId" FOREIGN KEY ("MessageReactionId") REFERENCES public."MessageReactions"("Id") ON DELETE CASCADE;


--
-- Name: MessageReactionUsers FK_MessageReactionUsers_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactionUsers"
    ADD CONSTRAINT "FK_MessageReactionUsers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: MessageReactions FK_MessageReactions_Messages_MessageId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MessageReactions"
    ADD CONSTRAINT "FK_MessageReactions_Messages_MessageId" FOREIGN KEY ("MessageId") REFERENCES public."Messages"("Id") ON DELETE CASCADE;


--
-- Name: Messages FK_Messages_Channels_ChannelId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages"
    ADD CONSTRAINT "FK_Messages_Channels_ChannelId" FOREIGN KEY ("ChannelId") REFERENCES public."Channels"("Id") ON DELETE CASCADE;


--
-- Name: Messages FK_Messages_Conversations_ConversationId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages"
    ADD CONSTRAINT "FK_Messages_Conversations_ConversationId" FOREIGN KEY ("ConversationId") REFERENCES public."Conversations"("Id");


--
-- Name: Messages FK_Messages_Users_SenderId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages"
    ADD CONSTRAINT "FK_Messages_Users_SenderId" FOREIGN KEY ("SenderId") REFERENCES public."Users"("Id");


--
-- Name: Messages FK_Messages_Workspaces_WorkspaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Messages"
    ADD CONSTRAINT "FK_Messages_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES public."Workspaces"("Id");


--
-- Name: UserInvitations FK_UserInvitations_Users_InvitedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserInvitations"
    ADD CONSTRAINT "FK_UserInvitations_Users_InvitedByUserId" FOREIGN KEY ("InvitedByUserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Users FK_Users_Workspaces_WorkspaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES public."Workspaces"("Id");


--
-- PostgreSQL database dump complete
--

